﻿using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyWithKeyDTO : AuthKey
    {
        public string KeyForAutorization { get; set; }

        public AuthKeyWithKeyDTO() : base() { }
        public AuthKeyWithKeyDTO(string key, string route, string keyForAutorization, string type, int paymentMicroserviceType) : base(key, route, type, 0, paymentMicroserviceType)
        {
            KeyForAutorization = keyForAutorization;
        }
    }
}
