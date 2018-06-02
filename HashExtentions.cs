using CryptLink.SigningFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptLink.ConsistentHash
{
    public static class HashExtentions {

        public static Hash Rehash(this Hash thisHash) {

            if (thisHash.Bytes == null) {
                throw new NullReferenceException("Byte array on hash was null, can't rehash");
            }

            return Hash.Compute(thisHash.Bytes, thisHash.Provider.Value, default(Cert));
        }

    }
}
