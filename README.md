# CryptLink.ConsistentHash
A high performance hash collection for use in a distributed hash table

To read about the concepts of a Consistent hashing, see (https://en.wikipedia.org/wiki/Consistent_hashing)

This project relies on (https://github.com/CryptLink/SigningFramework)

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)
[![Build status](https://ci.appveyor.com/api/projects/status/vhtrnq4m0ln13gpb?svg=true)](https://ci.appveyor.com/project/CryptLink/certbuilder)
[![NuGet](https://img.shields.io/nuget/v/CryptLink.CertBuilder.svg)](https://www.nuget.org/packages/CryptLink.CertBuilder/)

## Example
``` c#
    var c = new ConsistentHash<HashableString>(HashProvider.SHA384);
    var firstItem = new HashableString(Guid.NewGuid().ToString());
    firstItem.ComputeHash(c.Provider, null);

    //Adds a single item replicated 100 times
    c.Add(firstItem, true, 110);
```