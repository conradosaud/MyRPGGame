# My RPG Game
Este é um repositório de um protótipo de jogo no estilo RPG que estou projetando. A ideia é que este jogo possa ser tornar um MMORPG no futuro.

O sistema de combates do jogo é no estilo target clássico, onde você clica no inimigo para selecioná-lo e usa suas habilidades através do teclado númerico. Adotando o estilo clássico, é possível notar algumas limitações propositais, como por exemplo, não ser possível se mover enquanto executa a maioria das ações e sempre precisar de um alvo para realizar ações.

A movimentação é feita inteiramente através do mouse. Manter o clique do mouse pressionado faz com que o jogador ande, e o botão direito é usado para movimentar câmera. No momento não há intenções de fazer o personagem se locomover no ponto clicado, mas isso pode mudar.

Os detalhes do jogo e especificações do que ele pode se tornar no futuro está disponível no Game Design Document (GDD), que estou deixando neste repositório também como uma segunda documentação e como demonstração de _GDD_.

## Skills

O sistema de magias e habilidades do jogo, nomeado de _skill_, foi planejado de forma que possa ser escalável ao mesmo tempo que cada habilidade tenha uma espaço para ser programada e criada de qualquer forma imaginável.

Todas as skills são instâncias da classe [Skill](#), que por sua vez, contém todos os atributos e métodos necessários para se manipular uma habilidade do jogo, desde os efeitos até seus eventos como um todo. A classe Skill herda do componente [ScriptableObject](), da Unity, que permite criar arquivos customizados dentro da pasta do projeto com a descrição e atributos de cada skill usada no jogo.

As skills podem ser encontradas no diretório [Assets/Skills](#). Cada skill no jogo é composta por alguns arquivos padronizados com um sufixos, como exemplo vou usar a _Fireball_:
- __Fireball_Skill__ — representa uma instância da classe Skill com todas as informações que aquela skill tem (dano, alcance, alvo, etc). 
- Fireball_SkillPrefab — o que será disparado ao usar a habilidade, como por exemplo, o Transform de renderização da bola de fogo e suas partículas
- Fireball_ISkill — todo o código que cria a funcionalidade dessa skill, como por exemplo, perseguir um personagem, área de impacto, debuffs, etc
- Fireball_Animation — uma animação que será executada. Normalmente, todas as skills possuem uma resposta de quem as conjurou (_caster_), então essa animação normalmente é aplicada no próprio caster.

Esse padrão se mantém para todas as skills, embora haja a liberdade de criar-se algo diferente se necessário.

Vale destacar o uso da _interface_ [ISkill](#). Esta interface é implementada no script de evento de toda skill que explica como ela deve agir. A interface é usada para definir alguns métodos padrões que dá a certeza de que existirá em todas as skills, mantendo o padrão de comportamento esperado. Ainda, ela é necessário para ter certeza que a função _CastSkill_ da classe Skill possa passar sua referência para o objeto _SkillPrefab_.

### Fluxo de eventos

Em modo geral, uma skill passa pelas seguintes etapas, respectivamente:
1. A skill é adicionada na lista de skills possíveis do personagem usando o componente script [CharacterSkills](#)
2. Quando solicitada ao uso, é possível usar suas funções como _IsTargetInCasteRange_ e _IsSkillReady_ para verificar se a skill está disponível
3. A skill é lançada de forma definitiva através do método _CastSkill_
4. _CastSkill_ vai instanciar o objeto _SkillPrefab_ dentro da hierarquia de objetos chamada _SkillPool_ e passar todos os atributos da skill original para dentro desta instância
- O objeto _SkillPrefab_ deve conter o componente script *Fireball_ISKill* de interface dentro dele, obrigatoriamente
5. O script executa a função da interface chamada _ExecuteCharacterAnimation_ que a animação do _caster_ acontecer
6. A função _ExecuteCharacterAnimation_ chama a função _ExecuteSkillAnimation_ depois de determinado tempo. Essa função irá executar a animação da magia, como neste exemplo, a magia bolo de fogo vai começar a acontecer
7. Para magias de longo alcance, ela é finalizada quando atinge o alvo com _OnTriggerEnter_. Para skills instantâneas, isso pode acontecer junto do _ExecuteSkillAnimation_ ou em qualquer outro momento dentro do script. No final, o objeto e todos os eventos são destruídos

Sendo assim, resumidamente, as magias são divididas em quatro etapas:
1. Acionador do evento com _SkillCast_
2. Instanciação e atribuição de tudo que está na skill para dentro de _SkillPrefab_
3. Chamada da animação do caster com _ExecuteCharacterAnimation_
4. Chamada da animação da skill com _ExecuteSkillAnimation_

Para as skills, podem ser usados o modelo da skill [Fireball](#) para magias de longo alcance. Para magias de curto alcance (instantâneas ou que não perseguem o inimigo) podem ser usado o modelo [Sword Slash](#).

Mais detalhes e guia de uso no aqui [Readme.md no diretório Skill](#).
