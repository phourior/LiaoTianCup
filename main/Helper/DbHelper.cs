﻿using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace main.Helper;

public class DbHelper
{
    public static List<string> GetColumnData(string tableName, List<string> list)
    {
        using var connection = new SqliteConnection("Data Source=./Resources/database.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {tableName}";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(Convert.ToString(reader[0]));
            }
        }

        connection.Close();
        return list;
    }

    public static List<string[]> GetListData(string tableName,int columnCount, List<string[]> list)
    {
        using var connection = new SqliteConnection("Data Source=./Resources/database.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {tableName}";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                string[] rowStrings = new string[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    rowStrings[i] = Convert.ToString(reader[i]);
                }
                list.Add(rowStrings);
            }
        }
        connection.Close();
        return list;
    }

    public static List<string[]> GetCostListData(string tableName, int columnCount, List<string[]> list)
    {
        using var connection = new SqliteConnection("Data Source=./Resources/database.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {tableName} ORDER BY \"group\" DESC";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                string[] rowStrings = new string[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    rowStrings[i] = Convert.ToString(reader[i]);
                }
                list.Add(rowStrings);
            }
        }
        connection.Close();
        return list;
    }

    public static Dictionary<string, string> getDict_index_by_name()
    {
        Dictionary<string, string> dict = new Dictionary<string, string>();
        string table_name = "index_name";
        using var connection = new SqliteConnection("Data Source=./Resources/database.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {table_name}";

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                dict.Add(Convert.ToString(reader[1]), Convert.ToString(reader[0]));
            }
        }
        connection.Close();
        return dict;
    }
}