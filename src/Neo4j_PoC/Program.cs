using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Neo4j_PoC
{
    public class Program
    {
        private static IAsyncSession _session;

        public static async Task Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Neo4j PoC");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("This sample uses the default connection info (user/pass, host and database)");
            Console.WriteLine();

            // Get the nodes commands to be created on Neo4j
            var nodeCommands = GetCreateCommands();

            // Initialize Neo4j Connection
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Initializing Neo4j Connection...");
            InitializeNeo4jConnection("neo4j", "neo4j", "neo4j://localhost:7687", "neo4j");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Neo4j Initialized!");
            Console.WriteLine();

            try
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Creating nodes using Cypher...");
                Console.WriteLine();

                // Create each node
                foreach (var nodeCommand in nodeCommands)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Creating node with command: '{nodeCommand}'");
                    await CreateCommandAsync(nodeCommand);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Node created!");
                }
                Console.WriteLine();

                // Create the relation between nodes
                Console.ForegroundColor = ConsoleColor.Cyan;
                var relationCommand = "MATCH (p:Person), (pos:WorkPlace) WHERE p.department = pos.name CREATE (p)-[r:WORKS_AT]->(pos) RETURN type(r)";
                Console.WriteLine($"Creating relation between nodes with command: '{relationCommand}'");
                await CreateCommandAsync(relationCommand);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Relation created!");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Execute the command 'MATCH (n) RETURN n' in Neo4j Desktop to see the nodes and relations between them!");
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ResetColor();
            }

            Console.WriteLine("Press any key to close");
            Console.ReadKey();
        }

        private static void InitializeNeo4jConnection(string username, string password, string uri, string database)
        {
            IDriver driver = GraphDatabase.Driver(uri,
                                                  AuthTokens.Basic(username, password));
            _session = driver.AsyncSession(o => o.WithDatabase(database));
        }

        private static List<string> GetCreateCommands()
        {
            var nodeCommands = new List<string>();

            nodeCommands.Add("CREATE (pos:WorkPlace {name:'IT'})");
            nodeCommands.Add("CREATE (pos:WorkPlace {name:'Marketing'})");
            nodeCommands.Add("CREATE (p:Person {name:'Mike', department:'IT' })");
            nodeCommands.Add("CREATE (p:Person {name:'Rose', department:'IT' })");
            nodeCommands.Add("CREATE (p:Person {name:'John', department:'IT' })");
            nodeCommands.Add("CREATE (p:Person {name:'Mary', department:'Marketing' })");

            return nodeCommands;
        }

        private static async Task CreateCommandAsync(string command)
        {
            IResultCursor cursor = await _session.RunAsync(command);
            await cursor.ConsumeAsync();
        }
    }
}