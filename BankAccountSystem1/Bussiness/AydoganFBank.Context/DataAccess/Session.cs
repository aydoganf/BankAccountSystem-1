using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AydoganFBank.Context.DataAccess
{
    public class Session : ISession
    {
        private string token;
        private bool isValid;
        private DateTime validUntil;

        string ISession.Token => token;
        bool ISession.IsValid => isValid;

        void ISession.SetToken(string token, DateTime validUntil)
        {
            this.token = token;
            this.validUntil = validUntil;
            this.isValid = validUntil >= DateTime.Now;
        }
    }
}
