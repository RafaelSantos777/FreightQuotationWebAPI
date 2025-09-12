using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Microsoft.Graph.Users.Delta;
using UserService.Interfaces;
using UserService.Models.DTOs.Internal;

namespace UserService.Services;

public class UserQueryService(GraphServiceClient graphServiceClient) : IUserQueryService {

    private static readonly string[] GraphUserQueryParameters = ["id", "displayName", "mail"];

    public async Task<UserDTO?> GetUser(string id) {
        try {
            User user = (await graphServiceClient.Users[id].GetAsync(requestConfiguration => {
                requestConfiguration.QueryParameters.Select = GraphUserQueryParameters;
            }))!;
            return UserDTO.FromUser(user);
        }
        catch (ODataError oDataError) {
            if (oDataError.ResponseStatusCode == StatusCodes.Status404NotFound)
                return null;
            throw;
        }

    }

    // ReSharper disable once StringLiteralTypo
    public async Task<IEnumerable<UserDTO>> SearchUsers(string? search) {
        UserCollectionResponse? userCollectionResponse = await graphServiceClient.Users.GetAsync(requestConfiguration => {
            requestConfiguration.QueryParameters.Filter = search is null ? null : $"startswith(displayName,'{search}')";
            requestConfiguration.QueryParameters.Select = GraphUserQueryParameters;
        });
        if (userCollectionResponse is null)
            throw new Exception("Graph API returned null");
        return userCollectionResponse.Value is null ? [] : userCollectionResponse.Value.Select(UserDTO.FromUser);
    }

    public async Task<(IEnumerable<UserDTO> deletedUserDTOs, string nextDeltaLink)> GetDeletedUsers(string? deltaLink) {
        List<UserDTO> deletedUserDTOs = [];
        string? currentURL = deltaLink ?? null;
        while (true) {
            DeltaGetResponse deltaGetResponse = await GetDeltaResponse(currentURL);
            ProcessDeltaPage(deltaGetResponse);
            if (deltaGetResponse.OdataDeltaLink is not null)
                return (deletedUserDTOs, deltaGetResponse.OdataDeltaLink);
            currentURL = deltaGetResponse.OdataNextLink ?? throw new Exception("Graph API returned an invalid response.");
        }

        void ProcessDeltaPage(DeltaGetResponse deltaGetResponse) {
            foreach (User user in deltaGetResponse.Value ?? [])
                if (user.AdditionalData.ContainsKey("@removed"))
                    deletedUserDTOs.Add(UserDTO.FromUser(user));
        }

        async Task<DeltaGetResponse> GetDeltaResponse(string? url) {
            DeltaRequestBuilder requestBuilder = url is null
                ? graphServiceClient.Users.Delta
                : graphServiceClient.Users.Delta.WithUrl(url);
            DeltaGetResponse? deltaGetResponse = await requestBuilder.GetAsDeltaGetResponseAsync(requestConfiguration => {
                requestConfiguration.QueryParameters.Select = ["id"];
            });
            if (deltaGetResponse is null)
                throw new Exception("Graph API returned null");
            return deltaGetResponse;
        }
    }

}
