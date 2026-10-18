# Gerador de Senhas Fortes

## Problema
Quem precisa de uma senha forte deve conseguir gerar uma senha que atenda a critérios mínimos e recuperá-la posteriormente por um identificador.

## Objetivo
Permitir gerar senhas fortes, associá-las a um identificador único e consultar cada senha usando esse identificador.

## Usuários
- Consumidores da API que precisam gerar e consultar senhas.

## Histórias
- Como consumidor, quero solicitar uma senha forte para utilizá-la em outro contexto.
- Como consumidor, quero receber um identificador junto com a senha para consultá-la posteriormente.
- Como consumidor, quero consultar uma senha pelo identificador recebido.

## Requisitos funcionais
- RF-01: O sistema deve gerar uma senha quando receber uma solicitação de geração.
- RF-02: O sistema deve associar cada senha gerada a um GUID único.
- RF-03: O sistema deve armazenar a senha gerada associada ao seu GUID.
- RF-04: A resposta da geração deve informar a senha e seu GUID.
- RF-05: O sistema deve permitir consultar uma senha pelo GUID.
- RF-06: Quando o GUID existir, a consulta deve retornar a senha associada.

## Regras de negócio
- RN-01: Toda senha gerada deve ter pelo menos 16 caracteres.
- RN-02: Toda senha gerada deve conter pelo menos um caractere especial.
- RN-03: Nenhuma senha gerada pode conter espaços em branco.
- RN-04: Cada senha deve ser associada a um GUID único.
- RN-05: Um GUID válido sem senha associada deve resultar em resposta de recurso não encontrado.

## Casos de borda
- GUID em formato inválido deve ser rejeitado com HTTP 400.
- GUID válido, mas sem senha associada, deve resultar em HTTP 404.
- A senha retornada na consulta deve corresponder ao GUID informado.
- A geração de uma nova senha não deve substituir senhas já associadas a outros GUIDs.

## Fora de escopo
- Criptografia ou proteção das senhas armazenadas.
- Autenticação e autorização de usuários.
- Uso do sistema em produção.
- Definição de requisitos de composição além dos descritos nesta especificação.

## Critérios de aceite
- Ao solicitar uma senha, o sistema retorna a senha e um GUID.
- A senha retornada tem pelo menos 16 caracteres, contém ao menos um caractere especial e não contém espaços em branco.
- Ao consultar o GUID retornado, o sistema retorna a senha correspondente.
- Ao consultar um GUID válido sem associação, o sistema retorna HTTP 404.
- Ao consultar com um GUID em formato inválido, o sistema retorna HTTP 400 com `ProblemDetails`.