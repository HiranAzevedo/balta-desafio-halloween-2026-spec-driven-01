# Plano técnico

## Contexto
Implementar uma API didática que gera senhas e permite recuperá-las por GUID. A solução deve atender aos requisitos de `spec.md` e às regras arquiteturais e de qualidade de `constitution.md`.

## Arquitetura
- A solução e todos os projetos, arquivos de código-fonte e testes ficam dentro de `src/`: `src/PasswordGenerator.sln`, `src/PasswordGenerator.Api/` e `src/PasswordGenerator.Tests/`.
- Os endpoints da Minimal API recebem as solicitações e traduzem os resultados dos serviços para respostas HTTP.
- Um serviço de aplicação concentra a geração, validação e consulta das senhas.
- Um repositório isola a persistência e é consumido pelo serviço de aplicação.
- O repositório usa Entity Framework Core para persistir os dados em SQLite.
- A aplicação aplica as migrações pendentes do banco durante a inicialização, antes de aceitar requisições.
- O fluxo principal é: endpoint -> serviço de aplicação -> repositório -> banco de dados.
- A validação do formato do GUID ocorre na fronteira HTTP; regras de geração da senha ficam no serviço de aplicação.

## Decisões
- D-01: Usar C# com .NET 10 e Minimal API, conforme definido na constituição e recomendado para uma API pequena com dois recursos.
- D-02: Usar SQLite com Entity Framework Core. SQLite mantém a persistência local sem exigir um servidor de banco de dados separado; EF Core fornece o mapeamento e as operações de persistência.
- D-03: Usar xUnit para testes unitários e de integração, conforme a constituição. Adicionar `Microsoft.AspNetCore.Mvc.Testing` ao projeto de testes para executar os endpoints em um host de teste sem depender de um servidor externo.
- D-04: Gerar senhas com 16 caracteres nesta versão. Esse tamanho atende ao mínimo da especificação sem introduzir configuração de tamanho que não foi solicitada.
- D-05: Compor a senha a partir de letras maiúsculas, letras minúsculas, dígitos e caracteres especiais, garantindo ao menos um caractere especial e excluindo espaços em branco. Usar o gerador aleatório criptograficamente seguro disponibilizado pela plataforma, sem adicionar dependência externa.
- D-06: Expor a geração por `POST /api/passwords` e a consulta por `GET /api/passwords/{id}`. O primeiro retorna HTTP 201 com GUID e senha; o segundo retorna HTTP 200 com GUID e senha.
- D-07: Retornar HTTP 400 com `ProblemDetails` para GUID em formato inválido e HTTP 404 quando o GUID for válido, mas não existir. Isso segue a constituição e os critérios de aceite da especificação.
- D-08: Armazenar a senha sem criptografia, sem autenticação e sem autorização nesta versão, conforme o escopo didático definido na especificação. O sistema não deve ser usado em produção.
- D-09: Usar `Microsoft.EntityFrameworkCore.Design` e a ferramenta local `dotnet-ef`, registrada em `src/.config/dotnet-tools.json`, para criar e manter migrações versionadas do SQLite. São ferramentas de desenvolvimento necessárias para aplicar o modelo de dados de forma reproduzível e não fazem parte do runtime da API.
- D-10: Aplicar migrações pendentes com `Database.MigrateAsync()` durante a inicialização. Assim, `dotnet run` prepara o banco antes de iniciar a API, sem exigir um comando manual de migração; essa conveniência é apropriada ao escopo didático local, que não é destinado à produção.

## Modelo de dados
- Entidade `GeneratedPassword`:
	- `Id`: GUID, chave primária e identificador público da senha.
	- `Value`: senha gerada, campo obrigatório.
- Cada registro representa uma senha e seu identificador; não haverá atualização ou substituição de registros neste escopo.
- Uma migração do Entity Framework Core cria a estrutura necessária no SQLite.

## Contratos
- `POST /api/passwords`
	- Entrada: sem parâmetros ou corpo obrigatório.
	- Sucesso: HTTP 201; corpo JSON com `id` e `password`.
	- A resposta inclui o identificador que pode ser usado na consulta.
- `GET /api/passwords/{id}`
	- Entrada: GUID da senha no caminho da solicitação.
	- Sucesso: HTTP 200; corpo JSON com `id` e `password`.
	- GUID malformado: HTTP 400 com `ProblemDetails`.
	- GUID válido sem registro: HTTP 404.

## Riscos
- As senhas ficam armazenadas sem criptografia; isso é intencional apenas para o exercício e impede o uso em produção.
- O conjunto de caracteres e a regra de pelo menos um caractere especial devem ser cobertos por testes para evitar senhas fora dos critérios.
- O arquivo SQLite precisa permanecer disponível entre execuções para que as senhas persistidas possam ser consultadas posteriormente.
- A resposta de criação e a rota de consulta devem usar o mesmo formato de GUID para evitar inconsistências entre gerar e recuperar uma senha.