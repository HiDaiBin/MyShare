// 设置JWT的密钥
string secretKey = "your_secret_key";
byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);
var securityKey = new SymmetricSecurityKey(keyBytes);

// 创建JWT的签名凭证
var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

// 设置JWT的Claims
var claims = new[]
{
   new Claim(ClaimTypes.Name, "John Doe"),
   new Claim(ClaimTypes.Email, "john.doe@example.com"),
   // 添加其他需要的声明
};

// 创建JWT的Token
var token = new JwtSecurityToken(
   issuer: "your_issuer",
   audience: "your_audience",
   claims: claims,
   expires: DateTime.Now.AddDays(1),
   signingCredentials: signingCredentials
);

// 生成JWT字符串
var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);


// 验证JWT的密钥
var tokenValidationParameters = new TokenValidationParameters
{
   ValidateIssuer = true,
   ValidateAudience = true,
   ValidateIssuerSigningKey = true,
   IssuerSigningKey = securityKey,
   ValidIssuer = "your_issuer",
   ValidAudience = "your_audience"
};

// 验证JWT字符串
var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, tokenValidationParameters, out _);







//Nuget 安装JWT包
 
/// <summary>
/// 创建token
/// </summary>
/// <returns></returns>
public static string CreateJwtToken(IDictionary<string, object> payload, string secret)
{
    IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
    IJsonSerializer serializer = new JsonNetSerializer();
    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
    var token = encoder.Encode(payload, secret);
    return token;
}
/// <summary>
/// 校验解析token
/// </summary>
/// <returns></returns>
public static string ValidateJwtToken(string token, string secret)
{
    try
    {
        IJsonSerializer serializer = new JsonNetSerializer();
        IDateTimeProvider provider = new UtcDateTimeProvider();
        IJwtValidator validator = new JwtValidator(serializer, provider);
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtAlgorithm alg = new HMACSHA256Algorithm();
        IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, alg);
        var json = decoder.Decode(token, secret, true);
        //校验通过，返回解密后的字符串
        return json;
    }
    catch (TokenExpiredException)
    {
        //表示过期
        return "expired";
    }
    catch (SignatureVerificationException)
    {
        //表示验证不通过
        return "invalid";
    }
    catch (Exception)
    {
        return "error";
    }
}




//Payload 部分也是一个 JSON 对象，用来存放实际需要传递的数据。JWT 规定了7个官方字段，供选用。
//iss(issuer)：签发人
//exp(expiration time)：过期时间
//sub(subject)：主题
//aud(audience)：受众
//nbf(Not Before)：生效时间
//iat(Issued At)：签发时间
//jti(JWT ID)：编号
 
 
Dictionary<string, object> payload = new Dictionary<string, object>();
payload["uName"] = "lqwvje";
//过期时间(可以不设置，下面表示签名后 10秒过期)
payload["exp"] = Math.Ceiling((DateTime.UtcNow.AddSeconds(10) - new DateTime(1970, 1, 1)).TotalSeconds);
string s = CreateJwtToken(payload, "123456");//生成的token
 
 
string s2 = ValidateJwtToken(s, "123456");//校验解析token