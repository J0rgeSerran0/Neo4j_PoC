# Functional and Technical information of this simple PoC

We want to create some nodes in Neo4j, and one relation between them

The nodes and the relation between them, can be found in this image:

![](../documentation/images/Neo4j_PoC_Initial_Schema.png?raw=true)

> To draw the diagram, I have used [Draw.io](https://app.diagrams.net/)

The diagram can be found in the documentation folder [Neo4j_PoC.drawio](../documentation/diagram)

Here we have two entities as *WorkPlace* (Marketing and IT) and four entities as *Person* with two properties, a name and a department (Mary working in the Marketing workplace, and John, Mike and Rose working in the IT workplace).

Each entity will a node, so we will have to execute the [Cypher](https://neo4j.com/developer/cypher/) commands.

To create each node:

```charp
// Workplaces
CREATE (pos:WorkPlace {name:'IT'})
CREATE (pos:WorkPlace {name:'Marketing'})

// People
CREATE (p:Person {name:'Mike', department:'IT' })
CREATE (p:Person {name:'Rose', department:'IT' })
CREATE (p:Person {name:'John', department:'IT' })
CREATE (p:Person {name:'Mary', department:'Marketing' })
```

To create a relationship between them, we should execute the next command:

```charp
MATCH (p:Person), (pos:WorkPlace) WHERE p.department = pos.name CREATE (p)-[r:WORKS_AT]->(pos) RETURN type(r)
```

After execute all these commands, a new diagram will be created inside of Neo4j.

![](../documentation/images/Neo4j_PoC_Final_Diagram.png?raw=true)
