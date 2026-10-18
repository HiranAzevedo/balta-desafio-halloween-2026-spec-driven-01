# Tarefas

| ID | Tarefa | Origem | Depende de | Concluída quando |
|---|---|---|---|---|
| T-01 | Criar em `src/` a solução .NET 10 (`src/PasswordGenerator.sln`), o projeto da API (`src/PasswordGenerator.Api/`) e o projeto de testes (`src/PasswordGenerator.Tests/`) | Constituição, D-01, D-03 | | A solução compila e o projeto de testes é executável |
| T-02 | Implementar o gerador de senhas de 16 caracteres | RN-01, RN-02, RN-03, D-04, D-05 | T-01 | Testes unitários verificam tamanho, caracteres permitidos, presença de caractere especial e ausência de espaços |
| T-03 | Criar a entidade, o contexto de dados e a migração inicial | RF-02, RF-03, RN-04, D-02, D-09 | T-01 | A migração cria a estrutura para armazenar GUID e senha |
| T-04 | Implementar o repositório de senhas | RF-02, RF-03, RF-05, D-02 | T-03 | Teste verifica inclusão e consulta de uma senha persistida |
| T-05 | Implementar o serviço de aplicação para gerar, armazenar e consultar senhas | RF-01 a RF-06, RN-01 a RN-05 | T-02, T-04 | Testes unitários verificam geração, associação ao GUID, persistência e consulta |
| T-06 | Criar o endpoint de geração de senha | RF-01, RF-02, RF-03, RF-04, D-06 | T-05 | Teste de integração confirma resposta HTTP 201 com GUID e senha |
| T-07 | Criar o endpoint de consulta por GUID | RF-05, RF-06, D-06 | T-05 | Teste de integração confirma resposta HTTP 200 com a senha associada ao GUID |
| T-08 | Tratar GUID malformado na consulta | Caso de borda, Constituição, D-07 | T-07 | Teste de integração confirma HTTP 400 com `ProblemDetails` |
| T-09 | Tratar GUID válido sem senha associada | RN-05, caso de borda, D-07 | T-07 | Teste de integração confirma HTTP 404 |
| T-10 | Validar a aplicação completa | Constituição | T-01 a T-09 | Build sem erros ou warnings, todos os testes passam e a aplicação inicia sem erros |
