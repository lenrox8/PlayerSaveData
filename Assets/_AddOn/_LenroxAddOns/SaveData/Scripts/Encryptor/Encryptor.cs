using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Security.Cryptography;
using System.IO;

public static class Encryptor 
{
    public static string DecryptFromBase64(string base64, string key)
    {
        byte[] encryptedMessageBytes = Convert.FromBase64String(base64);
        byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);

        return Encryptor.Decrypt(encryptedMessageBytes, passwordBytes);
    }

    public static string Decrypt(byte[] encryptedMessageBytes, byte[] passwordBytes)
    {
        // Set encryption settings -- Use password for both key and init. vector
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        ICryptoTransform transform = provider.CreateDecryptor(passwordBytes, passwordBytes);
        CryptoStreamMode mode = CryptoStreamMode.Write;

        // Set up streams and decrypt
        MemoryStream memStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
        cryptoStream.Write(encryptedMessageBytes, 0, encryptedMessageBytes.Length);
        cryptoStream.FlushFinalBlock();

        // Read decrypted message from memory stream
        byte[] decryptedMessageBytes = new byte[memStream.Length];
        memStream.Position = 0;
        memStream.Read(decryptedMessageBytes, 0, decryptedMessageBytes.Length);

        string message = Encoding.UTF8.GetString(decryptedMessageBytes);

        return message;
    }


    public static string EncryptToBase64(string message, string key)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        byte[] passwordBytes = ASCIIEncoding.ASCII.GetBytes(key);

        byte[] encryptedMessage = Encryptor.Encrypt(messageBytes, passwordBytes);

        return Convert.ToBase64String(encryptedMessage);
    }

    public static byte[] Encrypt(byte[] messageBytes, byte[] passwordBytes)
    {
        // Set encryption settings -- Use password for both key and init. vector
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        ICryptoTransform transform = provider.CreateEncryptor(passwordBytes, passwordBytes);
        CryptoStreamMode mode = CryptoStreamMode.Write;

        // Set up streams and encrypt
        MemoryStream memStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memStream, transform, mode);
        cryptoStream.Write(messageBytes, 0, messageBytes.Length);
        cryptoStream.FlushFinalBlock();

        // Read the encrypted message from the memory stream
        byte[] encryptedMessageBytes = new byte[memStream.Length];
        memStream.Position = 0;
        memStream.Read(encryptedMessageBytes, 0, encryptedMessageBytes.Length);

        return encryptedMessageBytes;
    }


}
