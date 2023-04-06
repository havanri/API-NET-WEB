﻿using EFCoreExam.Request;

namespace EFCoreExam.Response
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
