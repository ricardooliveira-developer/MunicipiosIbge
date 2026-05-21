# Municípios IBGE

API para consultar municípios brasileiros usando dados públicos do IBGE.

O projeto mantém um snapshot local dos municípios em PostgreSQL e usa Redis como cache. As consultas seguem este fluxo:

1. Busca no cache.
2. Se não encontrar, busca no banco de dados.
3. Se o banco estiver vazio, sincroniza com a API pública do IBGE.
4. Salva os dados no banco e recria o cache.

## Tecnologias

- .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core
- PostgreSQL
- Redis, simulando AWS ElastiCache localmente
- Docker Compose
- Scalar para documentação OpenAPI
- xUnit para testes

## Pré-requisitos

Instale:

- [.NET SDK 10](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) ou Docker Engine com Docker Compose

Opcional:

- `dotnet-ef`, caso ainda não esteja instalado:

Windows:

```powershell
dotnet tool install --global dotnet-ef
```

Linux:

```bash
dotnet tool install --global dotnet-ef
```

## Configuração

As principais configurações ficam em:

```txt
src/MunicipiosIbge.Api/appsettings.json
src/MunicipiosIbge.Api/appsettings.Development.json
```

Configurações locais padrão:

```json
{
  "ConnectionStrings": {
    "MunicipalitiesDatabase": "Host=localhost;Port=5432;Database=municipios_ibge;Username=postgres;Password=postgres",
    "Cache": "localhost:6379"
  },
  "Ibge": {
    "BaseUrl": "https://servicodados.ibge.gov.br/",
    "TimeoutSeconds": 60
  },
  "Database": {
    "ApplyMigrationsOnStartup": true
  }
}
```

No Docker Compose, a API usa os nomes dos serviços internos:

```yaml
ConnectionStrings__MunicipalitiesDatabase: Host=postgres;Port=5432;Database=municipios_ibge;Username=postgres;Password=postgres
ConnectionStrings__Cache: elasticache:6379
Database__ApplyMigrationsOnStartup: true
Ibge__BaseUrl: https://servicodados.ibge.gov.br/
Ibge__TimeoutSeconds: 60
```

Com `Database:ApplyMigrationsOnStartup` habilitado, a API aplica automaticamente as migrations pendentes ao iniciar. Assim, ao subir a aplicação, as tabelas do PostgreSQL são criadas ou atualizadas sem precisar executar comandos manuais.

## Executando localmente com Docker

Suba o PostgreSQL e o Redis na raiz do projeto.

Windows:

```powershell
docker compose up -d postgres elasticache
```

Linux:

```bash
docker compose up -d postgres elasticache
```

Verifique se os containers estão saudáveis.

Windows:

```powershell
docker compose ps
```

Linux:

```bash
docker compose ps
```

Depois, execute a API localmente.

Windows:

```powershell
dotnet run --project src\MunicipiosIbge.Api\MunicipiosIbge.Api.csproj
```

Linux:

```bash
dotnet run --project src/MunicipiosIbge.Api/MunicipiosIbge.Api.csproj
```

Por padrão, o perfil `http` do `launchSettings.json` sobe a API em:

```txt
http://localhost:5101
```
## Acessando a documentação

Com a API rodando em ambiente de desenvolvimento, acesse o Scalar:

```txt
http://localhost:5101/scalar/v1
```

Também é possível acessar o documento OpenAPI:

```txt
http://localhost:5101/openapi/v1.json
```

Se você executar a API em outra porta, substitua `5101` pela porta exibida na saída do `dotnet run`.

Para executar explicitamente com o perfil HTTPS:

Windows:

```powershell
dotnet run --launch-profile https --project src\MunicipiosIbge.Api\MunicipiosIbge.Api.csproj
```

Linux:

```bash
dotnet run --launch-profile https --project src/MunicipiosIbge.Api/MunicipiosIbge.Api.csproj
```

Nesse caso, a documentação também estará disponível em:

```txt
https://localhost:7067/scalar/v1
https://localhost:7067/openapi/v1.json
```

## Endpoints principais

### Sincronizar municípios

Busca dados no IBGE, substitui o snapshot no banco e recria o cache.

```http
POST /sync
```

Exemplo de resposta:

```json
{
  "success": true,
  "data": {
    "totalReceived": 5571,
    "municipalitiesDeleted": 5570,
    "municipalitiesInserted": 5571,
    "cachedKeys": 5709
  },
  "message": "Municipalities synchronized successfully."
}
```

### Listar municípios

```http
GET /municipalities
```

Filtros opcionais:

```http
GET /municipalities?UF=MT
GET /municipalities?name=Sorriso
GET /municipalities?regionAcronym=CO
GET /municipalities?stateId=51
GET /municipalities?mesorregionId=5102
GET /municipalities?page=1&pageSize=20
```

Sem `page` e `pageSize`, a API retorna todos os registros encontrados.

### Buscar município por ID

```http
GET /municipalities/5101837
```

### Buscar municípios por mesorregião

```http
GET /municipalities/mesorregions/5102
GET /municipalities/mesorregions/5102?page=1&pageSize=20
```

## Formato de erro

Erros tratados seguem o formato:

```json
{
  "success": false,
  "message": "Mensagem explicando o erro."
}
```

Exemplos:

- `400`: parâmetros inválidos.
- `404`: município ou mesorregião não encontrado.
- `502`: erro ao consultar a API externa do IBGE.
- `500`: erro inesperado.

## Logs

Os logs no console são organizados por área:

```txt
2026-05-19 21:04:00 | HTTP   | INFO  | Requisição recebida: POST /sync.
2026-05-19 21:04:00 | SYNC   | INFO  | Sincronização iniciada: buscando municípios no IBGE.
2026-05-19 21:04:00 | IBGE   | INFO  | Chamando API do IBGE para buscar municípios.
2026-05-19 21:04:03 | DB     | INFO  | Limpando dados antigos do banco antes de inserir o novo snapshot.
2026-05-19 21:04:08 | CACHE  | INFO  | Cache recriado.
```

Queries SQL do Entity Framework não são exibidas por padrão.

## Testes

Rodar todos os testes.

Windows:

```powershell
dotnet test MunicipiosIbge.sln
```

Linux:

```bash
dotnet test MunicipiosIbge.sln
```

## Comandos úteis

Parar containers.

Windows:

```powershell
docker compose down
```

Linux:

```bash
docker compose down
```

Parar containers e remover volumes.

Windows:

```powershell
docker compose down -v
```

Linux:

```bash
docker compose down -v
```

Recriar containers.

Windows:

```powershell
docker compose up -d --build
```

Linux:

```bash
docker compose up -d --build
```

Ver logs dos containers.

Windows:

```powershell
docker compose logs -f
```

Linux:

```bash
docker compose logs -f
```
