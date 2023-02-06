using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

namespace Demo
{
    internal class Program
    {
        static async Task Main(string[] args)

        {
            SqlConnection conn = new SqlConnection(Configuration.CONNECTION_STRING);
            conn.Open();

            await using (conn){
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
        }
    }
}
