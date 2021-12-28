using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Common.Utils
{
    public class Token
    {
        public static string GenerarToken()
        {

            var claims = new[] {
                new Claim("numeroDocumento", "tiu88yiyuu"),
                new Claim(JwtRegisteredClaimNames.Typ, "1")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("pR0y3ct0&FPN3t_T3|ef0n1)@"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken("http://localhost:63969/",
                "pR0y3ct0&FPN3t_T3|ef0n1)@",
                claims,
                expires: DateTime.Now.AddMinutes(300),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
