﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class TokenResponse
    {
        public string? token_type { get; set; }

        public string? scope { get; set; }

        public int expires_in { get; set; }

        public int ext_expires_in { get; set; }

        public string? access_token { get; set; }

        public string? id_token { get; set; }
    }
}
