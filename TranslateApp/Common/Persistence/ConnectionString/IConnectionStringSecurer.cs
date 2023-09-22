﻿namespace TranslateApp.Common.Persistence.ConnectionString;

public interface IConnectionStringSecurer
{
    string? MakeSecure(string? connectionString, string? dbProvider = null);
}