using System;
using System.Text;
using UnityEngine;

[Serializable]
public class JwtPayload
{
    // Campos del JSON
    public int id;
    public string username;
    public string email;
    public string role;
    public long iat; 
    public long exp;
    public string iss;
    public string homepage;

    /// <summary>
    /// Método estático para crear una instancia de JwtPayload a partir de un string JWT.
    /// </summary>
    public static JwtPayload FromToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        // 1. Limpiar el prefijo "Bearer "
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            token = token.Substring(7).Trim();
        }

        // 2. Dividir el token
        string[] parts = token.Split('.');
        if (parts.Length < 2)
        {
            Debug.LogError("Token JWT inválido: No tiene el formato correcto.");
            return null;
        }

        try
        {
            // 3. Normalizar Base64Url a Base64 estándar
            string payloadEncoded = parts[1];
            payloadEncoded = payloadEncoded.Replace('-', '+').Replace('_', '/');
            
            switch (payloadEncoded.Length % 4)
            {
                case 2: payloadEncoded += "=="; break;
                case 3: payloadEncoded += "="; break;
            }

            // 4. Decodificar bytes a string JSON
            byte[] bytes = Convert.FromBase64String(payloadEncoded);
            string jsonText = Encoding.UTF8.GetString(bytes);

            // 5. Instanciar y retornar la clase mediante JsonUtility
            return JsonUtility.FromJson<JwtPayload>(jsonText);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al decodificar el JWT: {ex.Message}");
            return null;
        }
    }
}
