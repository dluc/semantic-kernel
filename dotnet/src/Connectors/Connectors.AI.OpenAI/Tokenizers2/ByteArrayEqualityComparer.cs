// Copyright (c) Microsoft. All rights reserved.

using System.Collections;
using System.Collections.Generic;

namespace Microsoft.SemanticKernel.Connectors.AI.OpenAI.Tokenizers2;

internal sealed class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
{
    public bool Equals(byte[]? x, byte[]? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return ReferenceEquals(x, y) || StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
    }

    public int GetHashCode(byte[] obj)
    {
        return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
}
