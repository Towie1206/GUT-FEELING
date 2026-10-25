public enum StoryFlag
{
    // Hồi 1: Đời thường & Điềm báo
    GameStarted,
    LuggageOpened,
    KeyPlacedOnTable,
    WaterBoiling,
    NoodleCooked,
    TVNewsWatched,
    NemCallFinished,
    SeenStrangerInRain,

    // Hồi 2: Thực tại sứt mẻ
    PowerOutageOccurred,
    FlashlightAcquired,
    FuseBoxOpened,
    RedFuseFound,
    YellowFuseFound,
    PowerRestored,

    // Hồi 3: Vạch trần & 3 Endings
    PhonePluggedIn,
    SecretWardrobeOpened,
    SecretWallSpaceEntered,
    FireExtinguisherAcquired,
    StalkerHitWithExtinguisher,

    // Endings
    BadEndingReached,
    NormalEndingReached,
    TrueEndingReached,

    // Cờ mở rộng chi tiết cho Hồi 2 & Hồi 3 (Khai báo nối tiếp ở cuối để KHÔNG làm lệch Index đã cấu hình trong Inspector)
    HalfEatenNoodleSeen,
    CreepyTVBroadcastSeen,
    RoomEscapeMinigameCompleted,
    BathroomSafeRevealed,
    BathroomSafeOpened,
    PhoneUnlocked,
    LastTenantMessageRead
}