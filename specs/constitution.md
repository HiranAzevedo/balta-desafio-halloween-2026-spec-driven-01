# Constituição do projeto

## Stack
- A aplicação será desenvolvida em C# com .NET 10 e Minimal API.
- A persistência será feita em SQLite usando Entity Framework Core.
- Os testes serão escritos com xUnit.

## Arquitetura
- Endpoints não acessam o banco de dados diretamente; usam serviços de aplicação.
- Regras de negócio não são implementadas em endpoints nem em handlers HTTP.
- O acesso a dados fica isolado em componentes próprios, acessados pelos serviços de aplicação.

## Qualidade
- Toda regra de negócio deve ter pelo menos um teste unitário.
- Todo endpoint deve ter pelo menos um teste de integração cobrindo o caminho feliz.
- Erros de validação devem retornar HTTP 400 com `ProblemDetails`.
- A aplicação deve compilar sem erros ou warnings, e todos os testes devem passar.

## Convenções
- Tipos e membros públicos usam `PascalCase`; parâmetros e variáveis locais usam `camelCase`.
- Testes devem identificar o cenário e o resultado esperado no nome.
- Operações de entrada e saída devem usar as APIs assíncronas disponíveis.
- Todos os artefatos da solução .NET, incluindo a solução, os projetos, o código-fonte e os testes, devem ficar dentro de `src/`. Os documentos do desafio e as especificações permanecem fora dessa pasta.

## Governança
- Toda dependência nova deve ser justificada no plano técnico.
- Mudanças devem estar relacionadas a requisitos da especificação e tarefas do plano.
- Alterações nesta constituição devem ser explícitas e acompanhadas de justificativa.
- Este projeto é didático e não deve ser usado como sistema de produção.