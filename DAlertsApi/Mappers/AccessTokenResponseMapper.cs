using DAlertsApi.DTOs;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Auth.AuthCode.Refresh;

namespace DAlertsApi.Mappers
{
    internal static class AccessTokenResponseMapper
    {
        public static AccessTokenResponse FromRefreshTokenResponseToAccessTokenResponse(this AccessTokenResponse accessTokenResponse, RefreshTokenResponse refreshTokenResponse)
        {
            accessTokenResponse.Access_token = refreshTokenResponse.Access_token;
            accessTokenResponse.Expires_in = refreshTokenResponse.Expires_in;
            accessTokenResponse.Token_type = refreshTokenResponse.Token_type;
            accessTokenResponse.Refresh_token = refreshTokenResponse.Refresh_token;
            return accessTokenResponse;
        }
        public static AccessTokenResponseDTO FromRefreshTokenResponseToAccessTokenResponseDTO(this AccessTokenResponseDTO accessTokenResponse, RefreshTokenResponse refreshTokenResponse)
        {
            return new AccessTokenResponseDTO
            {
                Access_token = refreshTokenResponse.Access_token,
                Expires_in = refreshTokenResponse.Expires_in,
                Token_type = refreshTokenResponse.Token_type,
                Refresh_token = refreshTokenResponse.Refresh_token,
                CreatedDate = DateTime.Now,
                ExperedDate = DateTime.Now.AddMilliseconds(refreshTokenResponse.Expires_in)
            };
        }
        public static AccessTokenResponseDTO FromAccessTokenResponseToAccessTokenResponseDTO(this AccessTokenResponse accessTokenResponse)
        {
            return new AccessTokenResponseDTO
            {
                Access_token = accessTokenResponse.Access_token,
                Expires_in = accessTokenResponse.Expires_in,
                Token_type = accessTokenResponse.Token_type,
                Refresh_token = accessTokenResponse.Refresh_token,
                CreatedDate = DateTime.Now,
                ExperedDate = DateTime.Now.AddMilliseconds(accessTokenResponse.Expires_in)
            };
        }
        public static AccessTokenResponse FromAccessTokenResponseDTOToAccessTokenResponse(this AccessTokenResponseDTO accessTokenResponseDTO)
        {
            return new AccessTokenResponse
            {
                Access_token = accessTokenResponseDTO.Access_token,
                Expires_in = accessTokenResponseDTO.Expires_in,
                Token_type = accessTokenResponseDTO.Token_type,
                Refresh_token = accessTokenResponseDTO.Refresh_token
            };
        }
    }
}
