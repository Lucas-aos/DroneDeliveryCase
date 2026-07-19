# Diário de Desenvolvimento

## Sessão 1 - Análise inicial do desafio

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

Foi definida a estratégia principal de formação das viagens, incluindo a escolha inicial por prioridade e peso, a avaliação de candidatos, o recálculo da rota, os critérios de desempate durante o agrupamento e o roteamento, a validação das restrições e as condições de encerramento.

Permaneceram pendentes a política de seleção dos drones e o desempate completo para escolha do primeiro pedido de cada viagem.

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

---

## Sessão 4 — Consolidação da especificação funcional

### Objetivo

Consolidar as decisões da estratégia de planejamento em uma especificação funcional completa, eliminando as ambiguidades restantes antes da modelagem da solução.

### Resultado

A especificação funcional do algoritmo foi consolidada.

Foram definidas as últimas regras que permaneciam em aberto, tornando o comportamento do planejamento determinístico e consolidando uma especificação funcional suficiente para implementação do MVP.

### Atividades realizadas

- Revisão da especificação funcional do algoritmo
- Identificação das lacunas restantes
- Comparação de alternativas para seleção dos drones
- Definição da política de seleção e reutilização dos drones
- Comparação de alternativas para o desempate na escolha do primeiro pedido
- Consolidação das decisões finais da estratégia de planejamento

### Decisão — Seleção e reutilização dos drones

Antes de cada nova viagem, serão identificados os drones capazes de atender individualmente pelo menos um pedido ainda pendente, considerando simultaneamente capacidade de peso e alcance necessário para sair da base, realizar a entrega e retornar à base.

Entre os drones elegíveis, será escolhido aquele com maior capacidade de peso.

Em caso de empate, serão utilizados, nesta ordem:

1. Maior alcance
2. Menor ordem de entrada
3. Menor identificador, quando aplicável

A seleção será repetida antes de cada nova viagem.

Todos os drones elegíveis voltarão a participar da seleção, permitindo reutilização ilimitada.

Não haverá rodízio, balanceamento, reserva ou preferência por drones ainda não utilizados.

### Justificativa

A política prioriza drones com maior potencial de agrupamento de pedidos, mantendo um comportamento simples, determinístico e coerente com o objetivo do MVP.

Por se tratar de uma heurística, não existe garantia de minimizar globalmente o número de viagens.

A escolha privilegia simplicidade e previsibilidade em detrimento de uma busca exaustiva pela solução ótima.

### Decisão — Desempate na escolha do primeiro pedido

Entre os pedidos atendíveis pelo drone selecionado, cada nova viagem será iniciada pelo pedido de maior peso dentro da maior prioridade disponível.

Quando dois ou mais pedidos possuírem a mesma prioridade e o mesmo peso, será escolhido aquele com menor ordem de entrada.

Persistindo o empate, será utilizado o menor identificador.

A ordem de entrada será preservada de forma estável e única durante todo o planejamento.

### Justificativa

A utilização da menor ordem de entrada mantém o comportamento previsível entre pedidos equivalentes, preserva a sequência de recebimento e evita introduzir uma nova heurística geográfica antes da formação da rota.

A decisão também mantém consistência com os demais critérios de desempate já utilizados pelo algoritmo.

### Decisão — Critérios de desempate em diferentes etapas do algoritmo

Durante a consolidação da especificação funcional foi avaliada a possibilidade de utilizar exatamente a mesma ordem de desempate em todos os pontos do algoritmo.

Optou-se por manter critérios diferentes entre a formação da viagem e a heurística do vizinho mais próximo.

Na formação da viagem, o peso permanece antes da ordem de entrada, pois pedidos mais pesados tendem a ser mais difíceis de acomodar dentro das restrições do drone.

Na ordenação dos destinos, todos os pedidos já pertencem à viagem.

Nessa etapa, a ordem de entrada passa a ter maior relevância por preservar previsibilidade entre destinos logisticamente equivalentes, enquanto o peso permanece apenas como critério secundário.

A diferença foi considerada intencional e passou a fazer parte da especificação funcional do algoritmo.

Essa decisão evita aplicar um mesmo critério em contextos cuja finalidade é diferente, preservando a coerência da heurística em cada etapa do planejamento.

### Limitações reconhecidas

- A política de seleção dos drones é heurística e não garante o menor número global de viagens
- A ordem de entrada não representa eficiência logística
- Alterações na sequência original dos pedidos podem modificar a composição das viagens, mesmo mantendo os mesmos dados de entrada
- Essas limitações são consideradas aceitáveis para o escopo do MVP, que prioriza simplicidade, previsibilidade e facilidade de validação
- A estratégia heurística não garante a melhor rota possível para cada viagem.

### Principais conclusões

- Todas as regras funcionais necessárias para o planejamento das viagens foram definidas
- Os critérios de desempate foram revisados individualmente conforme a finalidade de cada etapa do algoritmo
- O algoritmo possui comportamento determinístico para todos os cenários previstos no MVP
- As limitações conhecidas foram documentadas e aceitas como parte da estratégia heurística adotada

### Próximo passo

Validar a especificação funcional por meio de cenários representativos e, após sua aprovação, iniciar a modelagem técnica e a implementação.

---

## Sessão 5 — Modelagem conceitual do domínio

### Objetivo

Definir a estrutura conceitual do domínio antes da implementação, identificando os principais objetos, suas responsabilidades e seus relacionamentos, mantendo uma arquitetura simples e adequada ao escopo do MVP.

### Resultado

Foi concluída a modelagem conceitual do domínio, consolidada no documento `domain-model.md` e em seu respectivo diagrama de classes conceitual.

A modelagem foi refinada para representar apenas os conceitos essenciais do problema, separando claramente objetos do domínio, resultados do planejamento e componentes responsáveis pelo comportamento do algoritmo.

### Atividades realizadas

- Identificação dos principais objetos do domínio.
- Definição das responsabilidades de cada objeto.
- Definição dos relacionamentos entre os objetos.
- Avaliação da separação entre dados do domínio e componentes de comportamento.
- Revisão da modelagem para reduzir complexidade desnecessária.
- Comparação entre manter ou remover o objeto `Route`.
- Definição da representação dos pedidos impossíveis.
- Definição da prioridade como conjunto fechado de valores.
- Elaboração da documentação `domain-model.md`.
- Elaboração do diagrama de classes conceitual.

### Decisões tomadas

- O domínio será representado pelos objetos `Drone`, `Order`, `Coordinate`, `Trip`, `PlanningResult` e `ImpossibleOrder`.
- O objeto `Route` foi removido da modelagem por duplicar informações já representadas em `Trip`.
- A ordem da rota será representada pela sequência dos pedidos armazenados em cada viagem.
- A prioridade será representada por um conjunto fechado de valores (`High`, `Medium` e `Low`).
- Os pedidos impossíveis serão representados juntamente com o motivo da impossibilidade.
- O componente `TripPlanner` coordenará o fluxo do algoritmo.
- O cálculo de distância permanecerá isolado em `DistanceCalculator`.
- O recálculo da rota utilizando a heurística do Vizinho Mais Próximo será responsabilidade de `NearestNeighborRouteCalculator`.

### Limitações reconhecidas

- A modelagem representa apenas o domínio do problema, sem considerar persistência, API ou infraestrutura.
- Não foram introduzidas abstrações voltadas para extensões futuras que não façam parte do escopo do MVP.
- A distribuição definitiva das responsabilidades poderá sofrer pequenos ajustes durante a implementação, desde que preserve a estrutura conceitual definida.

### Principais conclusões

- A modelagem cobre todos os conceitos necessários para implementar a especificação funcional.
- As responsabilidades dos objetos foram definidas de forma clara e com baixo acoplamento.
- O domínio foi mantido independente de detalhes de infraestrutura.
- A documentação conceitual servirá como referência para a implementação das classes em C#.

### Lições aprendidas

A modelagem conceitual mostrou que representar corretamente o domínio é diferente de projetar a implementação. Antes de definir métodos, padrões ou detalhes técnicos, foi necessário identificar quais conceitos realmente pertencem ao problema de negócio e quais responsabilidades cada objeto deve assumir.

### Próximo passo

Projetar o modelo de implementação, definindo como os conceitos do domínio serão representados em C#, suas estruturas, tipos, encapsulamento e componentes de comportamento antes do início da implementação.

---

---

## Sessão 6 — Planejamento técnico da solução

### Objetivo

Definir a organização da solução e projetar como os conceitos do domínio serão representados em C#, antes do início da implementação.

### Resultado

Foi definida uma arquitetura simples para o MVP, composta por dois projetos de produção e dois projetos de testes.

Também foi concluído o projeto dos principais modelos de implementação, incluindo seus tipos, atributos, responsabilidades, encapsulamento, mutabilidade e regras de validação.

### Atividades realizadas

- Definição dos projetos que compõem a solution.
- Definição das responsabilidades e dependências entre os projetos.
- Definição da localização dos testes unitários e de integração.
- Avaliação da necessidade de uma camada de aplicação.
- Simplificação da arquitetura para evitar camadas sem responsabilidade concreta.
- Definição da representação dos modelos do domínio em C#.
- Comparação entre `class`, `record` e `record struct`.
- Definição da estratégia de imutabilidade dos modelos.
- Definição da representação da ordem de entrada de drones e pedidos.
- Avaliação da representação dos motivos de impossibilidade.
- Separação das responsabilidades de validação entre API, modelos do domínio e `TripPlanner`.
- Definição da exposição somente para leitura das coleções de resultados.

### Decisão — Estrutura da solution

A solution será organizada nos seguintes projetos:

```text
DroneDeliveryCase.sln

src/DroneDelivery.Domain + /DroneDelivery.Api/

tests/DroneDelivery.Domain.Tests/ + /DroneDelivery.Api.IntegrationTests/

```

O projeto `DroneDelivery.Domain` concentrará os modelos do domínio, o algoritmo de planejamento e os componentes auxiliares de cálculo.

O projeto `DroneDelivery.Api` será responsável pelos contratos HTTP, conversão entre contratos e domínio, configuração da aplicação e exposição do endpoint.

O projeto `DroneDelivery.Domain.Tests` conterá os testes unitários das regras do domínio e do algoritmo.

O projeto `DroneDelivery.Api.IntegrationTests` conterá os testes de integração do endpoint e do fluxo HTTP.

Não será criado um projeto `Application`, pois o MVP não possui casos de uso ou orquestrações adicionais que justifiquem uma camada intermediária. A API chamará diretamente o `TripPlanner`.


### Decisão — Representação dos modelos em C#

Os modelos serão representados da seguinte forma:

| Modelo | Representação |
|--------|---------------|
| `Drone` | `class` |
| `Order` | `class` |
| `Coordinate` | `readonly record struct` |
| `Trip` | `class` |
| `ImpossibleOrder` | `class` |
| `PlanningResult` | `class` |

`Coordinate` será representado como um tipo por valor porque não possui identidade própria, deve ser imutável e duas coordenadas com os mesmos valores representam a mesma posição.

Os demais modelos serão implementados como classes. Não será utilizado `sealed`, pois o domínio não possui hierarquias e restringir a herança não acrescentaria benefício relevante ao MVP.


### Decisão — Imutabilidade e encapsulamento

Os modelos serão criados em estados válidos e não serão alterados durante a execução do planejamento.

Não serão utilizados setters públicos para propriedades que fazem parte do estado dos objetos.

O `TripPlanner` poderá utilizar coleções e variáveis internas mutáveis durante a execução, mas os objetos de entrada e os resultados finais permanecerão estáveis.

As coleções de `Trip` e `PlanningResult` serão expostas somente para leitura e protegidas contra alterações externas.


### Decisão — Ordem de entrada

`Drone` e `Order` possuirão a propriedade `InputOrder`.

Esse valor é necessário para os critérios determinísticos de desempate definidos na especificação funcional.

O cliente não enviará `InputOrder` no contrato HTTP. A API atribuirá o valor com base na posição de cada elemento nas coleções de drones e pedidos recebidas na requisição.

Dessa forma, a ordem da coleção será a única fonte de verdade, enquanto o domínio preservará o valor durante toda a execução do algoritmo.


### Decisão — Motivo dos pedidos impossíveis

O motivo associado a um `ImpossibleOrder` será representado pelo enum `ImpossibleReason`, em vez de uma string livre.

A utilização de um conjunto fechado de valores evita inconsistências textuais, facilita testes e mantém as causas de impossibilidade explicitamente representadas no domínio.

A API poderá converter o valor para uma representação textual apropriada na resposta HTTP.


### Decisão — Validação

As validações serão distribuídas conforme a responsabilidade de cada componente.

A API será responsável por:

- validar a estrutura da requisição
- identificar campos obrigatórios ausentes
- validar a conversão dos valores recebidos
- converter os contratos HTTP em objetos do domínio.

Os construtores dos modelos serão responsáveis por impedir estados intrinsecamente inválidos, como:

- identificadores vazios
- pesos, capacidades e alcances inválidos
- coordenadas não finitas
- coleções nulas
- objetos de resultado incompletos.

O `TripPlanner` será responsável pelas validações que dependem do conjunto completo de drones e pedidos ou da execução do algoritmo, como:

- identificadores duplicados
- definição de pedidos impossíveis
- seleção de drones elegíveis
- respeito aos limites de capacidade e alcance
- garantia de que cada pedido apareça uma única vez no resultado
- consistência final do planejamento.

Serão utilizadas apenas exceções padrão de argumento do .NET para impedir a criação de objetos inválidos.

Pedidos impossíveis não serão tratados como exceções, pois representam um resultado esperado do planejamento.

### Principais conclusões

- A arquitetura da solution foi mantida simples e adequada ao escopo do MVP.
- O domínio permanecerá independente da API e de detalhes de infraestrutura.
- Não será criada uma camada de aplicação sem responsabilidade concreta.
- Os modelos serão predominantemente imutáveis.
- `Coordinate` terá semântica de valor.
- A ordem de entrada será preservada no domínio, mas derivada das coleções recebidas pela API.
- Os motivos de impossibilidade serão representados por um enum.
- As validações foram distribuídas entre API, construtores e `TripPlanner` conforme o conhecimento de cada componente.
- As decisões são suficientes para orientar a implementação sem alterar o modelo conceitual aprovado.

### Lições aprendidas

O planejamento técnico demonstrou que uma arquitetura com mais camadas não é necessariamente uma arquitetura melhor.

A estrutura da solução deve refletir responsabilidades reais do sistema. Para este MVP, manter o domínio independente e permitir que a API chame diretamente o planejador oferece organização suficiente sem introduzir abstrações que não agregariam valor.

Também foi possível distinguir a modelagem conceitual do projeto de implementação: o primeiro define os conceitos existentes no domínio, enquanto o segundo define como esses conceitos serão representados tecnicamente em C#.

### Próximo passo

Definir os enums utilizados pelo domínio e planejar os casos de teste que validarão as regras do algoritmo antes do início da implementação.

---

## Sessão 7 — Definição dos enums e planejamento dos testes

### Objetivo

Consolidar os últimos elementos do domínio antes da implementação e definir a estratégia de testes do MVP.

### Resultado

Foram definidos os enums utilizados pelo domínio e estabelecida a estratégia geral da suíte de testes, incluindo sua organização, prioridades e principais cenários.

### Atividades realizadas

- Definição dos enums do domínio.
- Avaliação da necessidade de valores neutros.
- Definição da precedência explícita das prioridades.
- Planejamento da suíte de testes.
- Separação entre testes unitários e de integração.
- Priorização dos cenários críticos.

### Decisões tomadas

- O domínio utilizará apenas os enums `Priority` e `ImpossibleReason`.
- Nenhum dos enums possuirá valores neutros.
- A precedência `High > Medium > Low` será aplicada explicitamente pela lógica do algoritmo.
- A maior parte da cobertura ficará em `DroneDelivery.Domain.Tests`.
- Os testes de integração ficarão concentrados em `DroneDelivery.Api.IntegrationTests`.
- Os testes serão implementados por prioridade, iniciando pelas regras centrais do algoritmo.

### Principais conclusões

- Todos os tipos fechados do domínio foram consolidados.
- A estratégia de testes cobre as principais regras funcionais e critérios de desempate.
- O planejamento está suficientemente definido para iniciar a implementação.

### Lições aprendidas

O planejamento da suíte mostrou que testes devem validar o comportamento definido pela especificação funcional, e não detalhes da implementação. A separação entre testes unitários e de integração contribui para uma validação mais clara e organizada do sistema.

### Próximo passo

Iniciar a implementação do domínio, desenvolvendo cada componente acompanhado pelos testes correspondentes de maior prioridade.

---

## Sessão 8 — Estrutura inicial e componentes básicos do domínio

### Objetivo

Iniciar a implementação incremental da solução, criando a estrutura física dos projetos e implementando os componentes mais básicos e independentes do domínio.

### Resultado

A solution foi criada com os quatro projetos aprovados, as referências foram configuradas e os primeiros componentes do domínio foram implementados e validados por testes unitários.

### Atividades realizadas

- Criação da solution `DroneDeliveryCase`;
- Criação dos projetos:
  - `DroneDelivery.Domain`;
  - `DroneDelivery.Api`;
  - `DroneDelivery.Domain.Tests`;
  - `DroneDelivery.Api.IntegrationTests`;
- Configuração das referências entre os projetos;
- Implementação de `Coordinate`;
- Implementação de `DistanceCalculator`;
- Implementação dos enums `Priority` e `ImpossibleReason`;
- Criação e execução dos testes unitários correspondentes;
- Validação do build completo da solution.

### Decisões tomadas

- `Coordinate` foi implementado como `readonly record struct`;
- coordenadas não finitas são rejeitadas com `ArgumentOutOfRangeException`;
- `DistanceCalculator` foi implementado como classe estática e sem estado;
- o cálculo de distância não realiza arredondamento;
- os enums não possuem valores neutros ou genéricos;
- os testes dos enums verificam apenas os valores aprovados, sem depender da ordem numérica;
- a implementação continuará de forma incremental, com uma responsabilidade por etapa.

### Principais conclusões

- A estrutura da solution está funcional;
- as referências entre projetos estão corretas;
- o domínio já possui uma base matemática e estrutural validada;
- 17 testes do domínio estão aprovados;
- nenhuma dependência prematura de entidades ou algoritmos foi introduzida.

### Lições aprendidas

- Validar a estrutura da solution antes da lógica reduz o risco de erros acumulados;
- componentes independentes devem ser implementados primeiro;
- testes pequenos facilitam a identificação de falhas;
- enums não devem ser usados implicitamente como regra de ordenação.

### Próximo passo

Implementar a entidade `Drone` e seus testes unitários, mantendo o padrão incremental adotado.

---

## Sessão 9 — Implementação da entidade `Drone`

### Objetivo

Implementar a primeira entidade do domínio, garantindo a criação de objetos sempre em estado válido e validando suas invariantes por meio de testes unitários.

### Resultado

A entidade `Drone` foi implementada de forma imutável, com validações realizadas no construtor para impedir estados inválidos. Todas as regras definidas durante o planejamento foram verificadas por testes unitários, mantendo a implementação consistente com o modelo de domínio aprovado.

### Atividades realizadas

- Criação da entidade `Drone`;
- Implementação das propriedades `Id`, `CapacityKg`, `RangeKm` e `InputOrder`;
- Implementação das validações do construtor;
- Validação de identificadores obrigatórios;
- Validação de capacidade e alcance como valores positivos e finitos;
- Validação da ordem de entrada;
- Criação dos testes unitários da entidade;
- Execução dos testes do domínio;
- Validação do build completo da solution.

### Decisões tomadas

- `Drone` foi implementado como uma `class` imutável;
- todas as propriedades são somente leitura;
- o construtor impede a criação de objetos inválidos;
- identificadores nulos, vazios ou compostos apenas por espaços são rejeitados;
- capacidade e alcance devem ser valores finitos maiores que zero;
- `InputOrder` deve ser maior ou igual a zero;
- foram utilizadas apenas exceções padrão do .NET para validação dos argumentos;
- as validações permaneceram explícitas na própria entidade, sem introduzir classes auxiliares de validação.

### Principais conclusões

- A primeira entidade do domínio foi implementada conforme o modelo conceitual aprovado;
- as invariantes do objeto são garantidas durante sua criação;
- o padrão de implementação adotado poderá ser reutilizado nas próximas entidades;
- a suíte de testes do domínio passou a conter **32 testes aprovados**;
- a solution permanece compilando sem erros.

### Lições aprendidas

Implementar entidades imutáveis com validações concentradas no construtor simplifica a garantia das invariantes do domínio e reduz a possibilidade de criação de objetos inconsistentes. A implementação incremental, acompanhada de testes unitários, facilita a identificação de problemas e estabelece uma base confiável para as próximas etapas do algoritmo.

### Próximo passo

Implementar a entidade `Order`, reutilizando os componentes já desenvolvidos (`Coordinate` e `Priority`) e mantendo o mesmo padrão de validação, imutabilidade e cobertura por testes unitários.

---

## Sessão 10 — Implementação da entidade `Order`

### Objetivo

Implementar a entidade responsável por representar um pedido individual do sistema, garantindo sua imutabilidade e impedindo a criação de pedidos com dados inválidos.

### Resultado

A entidade `Order` foi implementada com propriedades somente para leitura e validações realizadas no construtor. A implementação reutiliza os componentes `Coordinate` e `Priority`, mantendo as responsabilidades já definidas no modelo de domínio.

Todos os testes unitários da entidade foram executados com sucesso, e a suíte do domínio passou a conter **44 testes aprovados**.

### Atividades realizadas

- Criação da entidade `Order`;
- implementação das propriedades `Id`, `WeightKg`, `Destination`, `Priority` e `InputOrder`;
- reutilização do value object `Coordinate` para representar o destino;
- reutilização do enum `Priority`;
- implementação das validações do construtor;
- validação do identificador obrigatório;
- validação do peso como um valor positivo e finito;
- validação da prioridade recebida;
- validação da ordem de entrada;
- criação dos testes unitários da entidade;
- execução isolada dos testes de `Order`;
- execução completa dos testes do domínio;
- validação do build da solution.

### Decisões tomadas

- `Order` foi implementada como uma `class` imutável;
- todas as propriedades são somente para leitura;
- o destino é representado por `Coordinate`, evitando manter coordenadas separadas na entidade;
- as validações já garantidas por `Coordinate` não são repetidas em `Order`;
- o valor padrão de `Coordinate`, correspondente à posição `(0, 0)`, é aceito como destino válido;
- o identificador não pode ser nulo, vazio ou composto apenas por espaços;
- o peso deve ser um número finito maior que zero;
- a prioridade deve corresponder a um valor definido no enum `Priority`;
- `InputOrder` deve ser maior ou igual a zero;
- foram utilizadas exceções padrão do .NET;
- nenhuma regra de comparação, ordenação ou seleção de pedidos foi adicionada à entidade.

### Principais conclusões

- A entidade `Order` foi implementada conforme o modelo de domínio aprovado;
- suas invariantes são protegidas durante a construção;
- o uso de `Coordinate` tornou o modelo mais expressivo e evitou duplicação de conceitos;
- a validação explícita de `Priority` protege o domínio contra conversões inválidas de valores numéricos;
- a suíte de testes passou de **32 para 44 testes aprovados**;
- a solution continuou compilando sem erros.

### Lições aprendidas

A reutilização de value objects e enums já validados reduz duplicação de regras e deixa as entidades mais simples. Cada componente deve validar apenas as invariantes que pertencem à sua própria responsabilidade.

Também foi reforçada a importância de proteger o domínio contra valores que podem ser aceitos pelo compilador, mas que não representam estados válidos, como a conversão direta de um número não definido para um enum.

### Próximo passo

Implementar a entidade `Trip`, utilizando uma coleção somente para leitura de pedidos, realizando uma cópia defensiva da coleção recebida e mantendo as regras de capacidade, alcance e planejamento fora da entidade.

---

## Sessão 11 — Implementação da entidade `Trip`

### Objetivo

Implementar a entidade responsável por representar uma viagem, preservando a imutabilidade da coleção de pedidos e garantindo apenas as invariantes estruturais do objeto.

### Resultado

A entidade `Trip` foi implementada utilizando uma coleção somente para leitura, protegida por cópia defensiva. A entidade valida apenas sua consistência estrutural, mantendo as regras de capacidade, alcance e planejamento sob responsabilidade do `TripPlanner`.

A suíte do domínio passou a conter **61 testes aprovados**.

### Atividades realizadas

- Implementação da entidade `Trip`;
- criação da coleção somente leitura de pedidos;
- implementação da cópia defensiva da coleção recebida;
- validação dos argumentos do construtor;
- implementação dos testes unitários;
- execução completa da suíte;
- validação do build.

### Decisões tomadas

- `Trip` foi implementada como classe imutável;
- pedidos são expostos como `IReadOnlyList<Order>`;
- a coleção recebida é copiada antes de ser armazenada;
- coleções vazias não são permitidas;
- a entidade não valida capacidade, alcance ou consistência do planejamento.

### Principais conclusões

- O domínio passou a representar viagens completas;
- a responsabilidade entre entidade e algoritmo permaneceu bem definida;
- a suíte atingiu **61 testes aprovados**.

### Lições aprendidas

Coleções imutáveis reduzem o acoplamento e evitam alterações externas em objetos que representam resultados do domínio.

### Próximo passo

Implementar a entidade `ImpossibleOrder`.

---

## Sessão 12 — Implementação da entidade `ImpossibleOrder`

### Objetivo

Representar pedidos que não podem ser atendidos juntamente com o motivo da impossibilidade.

### Resultado

Foi implementada a entidade `ImpossibleOrder`, contendo o pedido e seu motivo de impossibilidade. A implementação mantém o padrão de imutabilidade utilizado pelas demais entidades do domínio.

A suíte passou a conter **67 testes aprovados**.

### Atividades realizadas

- Implementação da entidade;
- validação do pedido recebido;
- validação do enum `ImpossibleReason`;
- criação dos testes unitários;
- execução da suíte completa.

### Decisões tomadas

- A entidade apenas representa um resultado;
- não verifica se o motivo informado está correto;
- a validação da lógica permanece responsabilidade do `TripPlanner`.

### Principais conclusões

- Todos os conceitos básicos do domínio passaram a estar representados;
- a suíte atingiu **67 testes aprovados**.

### Lições aprendidas

Nem toda entidade deve conter regras de negócio complexas. Algumas existem apenas para representar estados válidos do domínio.

### Próximo passo

Implementar o agregado `PlanningResult`.

---

## Sessão 13 — Implementação do agregado `PlanningResult`

### Objetivo

Implementar o agregado responsável por representar o resultado completo do planejamento das viagens.

### Resultado

Foi implementado o agregado `PlanningResult`, contendo as viagens planejadas e os pedidos impossíveis.

As coleções são expostas somente para leitura, protegidas por cópia defensiva e validadas estruturalmente.

A suíte passou a conter **79 testes aprovados**.

### Atividades realizadas

- Implementação do agregado;
- implementação das coleções somente leitura;
- cópia defensiva das coleções;
- validação dos argumentos;
- implementação dos testes unitários;
- execução completa da suíte.

### Decisões tomadas

- O agregado não valida regras do algoritmo;
- coleções vazias são permitidas;
- apenas integridade estrutural é verificada.

### Principais conclusões

- Todo o modelo de domínio passou a estar implementado;
- a suíte atingiu **79 testes aprovados**.

### Lições aprendidas

Agregados não precisam repetir regras cuja responsabilidade pertence ao componente que os produz.

### Próximo passo

Implementar o algoritmo de roteamento utilizando a heurística do Vizinho Mais Próximo.

---

## Sessão 14 — Implementação do algoritmo de roteamento

### Objetivo

Implementar o cálculo da rota utilizando a heurística do Vizinho Mais Próximo e retornar a sequência ordenada de pedidos juntamente com a distância total da viagem.

### Resultado

Foram implementados `RouteCalculationResult` e `NearestNeighborRouteCalculator`.

O algoritmo inicia a rota na base `(0,0)`, seleciona iterativamente o pedido mais próximo da posição atual e inclui explicitamente o retorno à base no cálculo da distância total.

Todos os critérios determinísticos de desempate definidos na especificação funcional foram implementados e validados.

A suíte completa do domínio passou a conter **100 testes aprovados**.

### Atividades realizadas

- Criação da pasta `Planning`;
- implementação de `RouteCalculationResult`;
- implementação de `NearestNeighborRouteCalculator`;
- utilização de `DistanceCalculator`;
- implementação da heurística do Vizinho Mais Próximo;
- implementação dos critérios determinísticos de desempate;
- implementação dos testes unitários;
- execução da suíte completa;
- validação do build.

### Decisões tomadas

- O algoritmo possui responsabilidade exclusivamente geométrica;
- drones, capacidade, alcance e pedidos impossíveis permanecem fora deste componente;
- a distância total inclui o retorno à base;
- a ordem de enumeração da coleção nunca é utilizada como critério de desempate;
- a prioridade é comparada explicitamente, sem depender do valor numérico do enum.

### Principais conclusões

- O roteamento tornou-se totalmente determinístico;
- todos os critérios definidos na especificação funcional foram implementados;
- o domínio passou a conter todos os componentes necessários para iniciar o planejamento das viagens;
- a suíte atingiu **100 testes aprovados**.

### Lições aprendidas

Separar o cálculo da rota do planejamento das viagens simplifica o domínio, aumenta a reutilização do algoritmo e torna os testes significativamente mais objetivos.

### Próximo passo

Iniciar a implementação incremental do `TripPlanner`, responsável por orquestrar todo o algoritmo de planejamento das viagens.