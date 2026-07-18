# Diário de Desenvolvimento

## Sessão 1

### Objetivo

Entender completamente o problema antes de iniciar a implementação.

### Resultado

Foi realizada a análise inicial do enunciado, identificando requisitos, diferenciais, funcionalidades opcionais e as principais ambiguidades que precisarão ser resolvidas antes da implementação.

### Atividades realizadas

- Leitura e interpretação do enunciado do desafio.
- Separação entre regras obrigatórias, diferenciais e funcionalidades opcionais.
- Identificação do objetivo principal do sistema e das informações de entrada e saída.
- Levantamento das principais ambiguidades presentes no enunciado.
- Criação do documento `assumptions.md` para registrar questões em aberto e futuras decisões de projeto.

### Principais conclusões

- Existem regras obrigatórias.
- Existem diferenciais.
- Existem funcionalidades opcionais.
- Foram identificadas as principais decisões necessárias para definir o comportamento do MVP.
- Ainda existem ambiguidades importantes que precisam ser resolvidas antes da modelagem.

### Próximo passo

Analisar as ambiguidades do enunciado, avaliar as alternativas para cada uma delas e registrar as primeiras premissas do projeto.

---

## Sessão 2

### Objetivo

Analisar as principais ambiguidades do enunciado e definir as primeiras premissas do projeto para orientar o desenvolvimento do MVP.

### Resultado

Foram avaliadas diferentes alternativas para os principais pontos não especificados no enunciado. Parte das ambiguidades foi resolvida e transformada em premissas do projeto, enquanto outras permaneceram em análise.

### Atividades realizadas

- Análise do significado de uma viagem.
- Avaliação das alternativas para definição da base dos drones.
- Avaliação da necessidade de retorno à base.
- Comparação entre fórmulas para cálculo da distância.
- Definição do cálculo da distância total de uma viagem.
- Avaliação das alternativas para utilização da prioridade dos pedidos.
- Definição do nível de otimização esperado para o MVP.
- Avaliação da reutilização dos drones durante a simulação.
- Definição da estratégia para tratamento de pedidos impossíveis.
- Separação entre interpretações do enunciado e decisões de projeto.

### Principais conclusões

- Uma viagem poderá agrupar vários pedidos e visitar vários destinos.
- Todos os drones partirão de uma base única localizada em `(0,0)`.
- Toda viagem considerará o retorno do drone à base.
- Será utilizada distância euclidiana para calcular o deslocamento entre coordenadas.
- A distância total incluirá todos os trechos percorridos durante a viagem.
- Os pedidos serão inicialmente considerados em ordem de prioridade, de Alta para Média e Baixa.
- Pedidos com prioridades diferentes poderão ser agrupados na mesma viagem.
- O MVP utilizará uma heurística simples para reduzir o número de viagens.
- Um mesmo drone poderá realizar várias viagens.
- Pedidos impossíveis serão tratados separadamente.
- Permaneceu em análise a estratégia de ordenação dos destinos dentro de uma viagem.

### Próximo passo

Definir a estratégia de ordenação dos destinos dentro de uma viagem e os critérios de desempate antes de iniciar a modelagem da solução.