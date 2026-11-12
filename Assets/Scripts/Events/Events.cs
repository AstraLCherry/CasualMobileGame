public struct OnPlayerSpawn : IEvent
{
    public Player Player { get; private set; }

    public OnPlayerSpawn(Player player) => Player = player;
}

public struct OnPlayerDashing : IEvent
{
    public PlayerMovement PlayerMovement { get; private set; }

    public OnPlayerDashing(PlayerMovement playerMovement) => PlayerMovement = playerMovement;
}

public struct OnPlayerStartAim : IEvent
{
    public Player Player { get; private set; }

    public OnPlayerStartAim(Player player) => Player = player;
}

public struct OnPlayerUpdateAim : IEvent
{
    public Player Player { get; private set; }

    public OnPlayerUpdateAim(Player player) => Player = player;
}

public struct OnPlayerReleaseAim : IEvent
{
    public Player Player { get; private set; }

    public OnPlayerReleaseAim(Player player) => Player = player;
}
