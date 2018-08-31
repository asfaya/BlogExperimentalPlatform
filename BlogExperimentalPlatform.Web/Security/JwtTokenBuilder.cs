namespace BlogExperimentalPlatform.Web.Security
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;

    public sealed class JwtTokenBuilder
    {
        #region Members
        private SecurityKey securityKey = null;
        private string subject = string.Empty;
        private string issuer = string.Empty;
        private string audience = string.Empty;
        private Dictionary<string, string> claims = new Dictionary<string, string>();
        private int expiryInMinutes = 600;
        #endregion

        #region Methods
        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this.securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this.subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this.issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this.audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            foreach (var claim in claims)
                this.claims.Add(claim.Key, claim.Value);

            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            this.expiryInMinutes = expiryInMinutes;
            return this;
        }

        public JwtToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, subject),
              new Claim(ClaimTypes.Name, subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(this.claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: issuer,
                              audience: audience,
                              claims: claims,
                              expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                              signingCredentials: new SigningCredentials(
                                                        securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return new JwtToken(token);
        }
        #endregion

        #region Private
        private void EnsureArguments()
        {
            if (securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(subject))
                throw new ArgumentNullException("Subject");

            if (string.IsNullOrEmpty(issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(audience))
                throw new ArgumentNullException("Audience");
        }
        #endregion
    }
}
