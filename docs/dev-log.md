# Diário de Desenvolvimento

## Sessão 1 - Análise iniciald o desafio

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
- Foram identificadas as principais decisões necessárias para definir o comportamento do MVP (Minimun Viable Product).
- Ainda existem ambiguidades importantes que precisam ser resolvidas antes da modelagem.

### Próximo passo

Analisar as ambiguidades do enunciado, avaliar as alternativas para cada uma delas e registrar as primeiras premissas do projeto.

---

## Sessão 2 - Definição das premissas do MVP

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

---

## Sessão 3 — Definição da estratégia de planejamento

### Objetivo

Definir a heurística utilizada para formar viagens, selecionar pedidos adicionais, organizar os destinos e validar o alcance dos drones.

### Resultado

Foi definida a estratégia completa de formação das viagens, incluindo a escolha do primeiro pedido, a avaliação de candidatos, o recálculo da rota, os critérios de desempate, a validação das restrições e as condições de encerramento.

### Atividades realizadas

- Comparação de alternativas para escolha do primeiro pedido da viagem
- Análise de critérios para agrupamento de pedidos
- Definição da heurística do vizinho mais próximo para organização da rota
- Análise da relação entre seleção de candidatos e recálculo da rota
- Comparação entre avaliação pela ordem atual, melhor inserção e recálculo completo da heurística
- Definição das condições de encerramento de uma viagem
- Identificação das limitações da estratégia escolhida.

### Decisões tomadas

- Os pedidos são inicialmente considerados por prioridade
- Uma viagem começa pelo pedido mais pesado dentro da maior prioridade disponível
- Cada pedido candidato é incluído temporariamente para avaliação
- A rota temporária é recalculada utilizando o vizinho mais próximo
- O cálculo da distância inclui o retorno à base
- Candidatos que ultrapassarem peso ou alcance são desconsiderados para a viagem atual, mas permanecem pendentes para avaliações posteriores.
- Entre os candidatos válidos, é escolhido o que produzir o menor aumento de distância
- Os desempates entre candidatos seguem maior prioridade, maior peso, menor ordem de entrada e menor identificador.
- A rota simulada do candidato escolhido torna-se a rota oficial da viagem
- A viagem termina quando nenhum pedido adicional puder ser incluído validamente.

### Limitações reconhecidas

- O vizinho mais próximo não garante a menor rota
- A escolha local do menor aumento não garante o menor número global de viagens
- A heurística poderá executar vários recálculos de rota
- O custo adicional é considerado aceitável para o volume esperado no MVP.

### Decisão complementar

Foi definido que empates durante a execução da heurística do vizinho mais próximo serão resolvidos pela seguinte ordem:

1. Maior prioridade
2. Menor ordem de entrada
3. Maior peso
4. Menor identificador.

A prioridade preserva a urgência dos pedidos, enquanto a ordem de entrada favorece previsibilidade e evita ultrapassagens desnecessárias.

O peso foi mantido como terceiro critério para preservar a consistência com a estratégia geral de tratar pacotes mais pesados mais cedo. O identificador será utilizado apenas para garantir determinismo em caso de empate completo.

### Próximo passo

Descrever o algoritmo completo de planejamento em linguagem natural. Em seguida, validar seu comportamento por meio de cenários pequenos antes de iniciar a modelagem técnica.