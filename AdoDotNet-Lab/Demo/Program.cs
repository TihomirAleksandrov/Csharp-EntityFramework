using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo
{
    internal class Program
    {
        static async Task Main(string[] args)

        {
            SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING);
            conn.Open();

            int villainId = int.Parse(Console.ReadLine());

            await using (conn){
                await PrintVillainMinionsByVillainIdAsync(conn, villainId);
            }
        }

        //Task2
        private static async Task PrintVillainsWithMoreThan3MinionsAsync(SqlConnection conn)
        {
            SqlCommand filterDb = new SqlCommand(Queries.VillainsWithMoreThan3Minions, conn);

            SqlDataReader reader = await filterDb.ExecuteReaderAsync();

            await using (reader)
            {
                while (await reader.ReadAsync())
                {
                    string villainName = reader.GetString(0);
                    int minionsCount = reader.GetInt32(1);

                    Console.WriteLine($"{villainName} - {minionsCount}");
                }
            }
        }

        //Task3
        private static async Task PrintVillainMinionsByVillainIdAsync(SqlConnection conn, int villainId)
        {
            SqlCommand commandVillainId = new SqlCommand(Queries.VillainNameById, conn);
            commandVillainId.Parameters.AddWithValue("@Id", villainId);

            object villainNameObject = await commandVillainId.ExecuteScalarAsync();

            if (villainNameObject == null)
            {
                Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                return;
            }

            string villainName = (string)villainNameObject;

            SqlCommand commandVillainMinions = new SqlCommand(Queries.VillainWithMinionsInfo, conn);
            commandVillainMinions.Parameters.AddWithValue("@Id", villainId);

            SqlDataReader reader = await commandVillainMinions.ExecuteReaderAsync();

            await using (reader)
            {
                Console.WriteLine($"Villain: {villainName}");

                if (!reader.HasRows)
                {
                    Console.WriteLine("(no minions)");
                }
                else
                {
                    List<string> minionsCollection = new List<string>();

                    while (await reader.ReadAsync())
                    {
                        long rowNumber = reader.GetInt64(0);
                        string minionName = reader.GetString(1);
                        int minionAge = reader.GetInt32(2);

                        minionsCollection.Add($"{rowNumber}. {minionName} {minionAge}");
                    }

                    foreach (var minion in minionsCollection)
                    {
                        Console.WriteLine(minion);
                    }
                }
            }
        }
    }
}
