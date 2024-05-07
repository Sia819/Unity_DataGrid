using System;

public class UUIDGenerator
{
    public static string CreateUUID()
    {
        Guid newGuid = Guid.NewGuid();
        return newGuid.ToString();
    }
}
