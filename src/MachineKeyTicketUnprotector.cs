﻿using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;

namespace AspNetTicketBridge
{
    public static class MachineKeyTicketUnprotector
    {
        /// <summary>
        /// Decodes, decrypts and deseralizes a serialized, protected and encoded 
        /// AuthenticationTicket created by OWIN's OAuth server implementation for the access token.
        /// </summary>
        /// <param name="token">The token generated by OWIN (not not include 'bearer')</param>
        /// <param name="decryptionKey">The machineKey decryptionKey found in your web.config</param>
        /// <param name="validationKey">The machineKey validationKey found in your web.config</param>
        /// <param name="decryptionAlgorithm">The machineKey decryptionAlgorithm found in your web.config (Auto == AES)</param>
        /// <param name="validationAlgorithm">The machineKey validationAlgorithm found in your web.config</param>
        /// <returns>A v3 AuthenticationTicket</returns>
        public static OwinAuthenticationTicket UnprotectOAuthToken(string token, string decryptionKey, string validationKey,
            string decryptionAlgorithm = "AES", string validationAlgorithm = "HMACSHA1")
        {
            var decoded = WebEncoders.Base64UrlDecode(token);

            var unprotected = Unprotect(decoded, decryptionKey, validationKey,
                decryptionAlgorithm, validationAlgorithm,
                "User.MachineKey.Protect",
                "Microsoft.Owin.Security.OAuth", "Access_Token", "v1");

            if (unprotected == null)
            {
                return null;
            }
            var serializer = new OwinTicketSerializer();
            var ticket = serializer.Deserialize(unprotected);
            return ticket;
        }

        /// <summary>
        /// Decodes, decrypts and deseralizes a serialized, protected and encoded 
        /// AuthenticationTicket created by OWIN's OAuth server implementation for the refresh token.
        /// </summary>
        /// <param name="token">The token generated by OWIN</param>
        /// <param name="decryptionKey">The machineKey decryptionKey found in your web.config</param>
        /// <param name="validationKey">The machineKey validationKey found in your web.config</param>
        /// <param name="decryptionAlgorithm">The machineKey decryptionAlgorithm found in your web.config (Auto == AES)</param>
        /// <param name="validationAlgorithm">The machineKey validationAlgorithm found in your web.config</param>
        /// <returns></returns>
        public static OwinAuthenticationTicket UnprotectOAuthRefreshToken(string token, string decryptionKey, string validationKey,
            string decryptionAlgorithm = "AES", string validationAlgorithm = "HMACSHA1")
        {
            var decoded = WebEncoders.Base64UrlDecode(token);

            var unprotected = Unprotect(decoded, decryptionKey, validationKey,
                decryptionAlgorithm, validationAlgorithm,
                "User.MachineKey.Protect",
                "Microsoft.Owin.Security.OAuth", "Refresh_Token", "v1");
            if (unprotected == null)
            {
                return null;
            }
            var serializer = new OwinTicketSerializer();
            var ticket = serializer.Deserialize(unprotected);
            return ticket;
        }

        /// <summary>
        /// Decodes, decryptes and deseralizes a serialized, protected and encoded 
        /// OwinAuthenticationTicket created by OWIN's cookie authentication system.
        /// </summary>
        /// <param name="cookie">The cookie value generated by OWIN</param>
        /// <param name="decryptionKey">The machineKey decryptionKey found in your web.config</param>
        /// <param name="validationKey">The machineKey validationKey found in your web.config</param>
        /// <param name="decryptionAlgorithm">The machineKey decryptionAlgorithm found in your web.config (Auto == AES)</param>
        /// <param name="validationAlgorithm">The machineKey validationAlgorithm found in your web.config</param>
        /// <returns>A v3 AuthenticationTicket</returns>
        public static OwinAuthenticationTicket UnprotectCookie(string cookie, string decryptionKey, string validationKey,
            string decryptionAlgorithm = "AES", string validationAlgorithm = "HMACSHA1")
        {
            var decoded = WebEncoders.Base64UrlDecode(cookie);

            var unprotected = Unprotect(decoded, decryptionKey, validationKey,
                decryptionAlgorithm, validationAlgorithm,
                "User.MachineKey.Protect",
                "Microsoft.Owin.Security.Cookies.CookieAuthenticationMiddleware", "ApplicationCookie", "v1");
            if (unprotected == null)
            {
                return null;
            }
            var serializer = new OwinTicketSerializer();
            var ticket = serializer.Deserialize(unprotected);
            return ticket;
        }

        public static byte[] Unprotect(byte[] decodedData, string decryptionKey, string validationKey,
            string decryptionAlgorithm, string validationAlgorithm,
            string primaryPurpose, params string[] purposes)
        {
            var unprotectedData = MachineKey.Unprotect(decodedData, validationKey, decryptionKey,
                decryptionAlgorithm, validationAlgorithm,
                primaryPurpose, purposes);
            return unprotectedData;
        }
    }
}
