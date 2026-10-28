# MASTER BLUEPRINT TUẦN 11 - 12: CHUỖI VIỆC NHÀ HỒI 1, TIVI CRT & CUTSCENE BAN CÔNG (TIMELINE + CINEMACHINE)

> **Mục tiêu Tuần 11 - 12:**  
> Hoàn thiện toàn bộ nhịp gameplay Đời thường & Điềm báo của **Hồi 1** dựa trên đúng hệ thống Interface ([`IInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IInteractable.cs), [`IHoldInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldInteractable.cs), [`IHoldable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldable.cs), [`IPlaceable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IPlaceable.cs), [`IAltUsable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IAltUsable.cs), [`ItemSocket`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs)) và [`StoryEventBus`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Story/StoryEventBus.cs) đã xây dựng.  
> Tài liệu này được nâng cấp chuyên sâu sau khi giải mã và đối chiếu kỹ thuật trực tiếp từ bộ **5 Video Tutorial trọng tâm của kênh BatPan (YouTuber xây dựng series Fears to Fathom in Unity)**.  
> Tất cả mã nguồn dưới đây được thiết kế dưới dạng **SOLID Form / Skeleton** khớp 100% với chữ ký hàm và kiến trúc của dự án GUT FEELING để bạn tự tay triển khai phần `// TODO:`.

---

## MỤC LỤC BLUEPRINT
1. **[PHẦN 0: ĐỐI CHIẾU 5 VIDEO CỦA ANH BATPAN & BẢN ĐỒ CHUYỂN HÓA SANG SOLID](#phần-0-đối-chiếu-5-video-của-anh-batpan--bản-đồ-chuyển-hóa-sang-solid)**
   - 0.1. Danh mục 5 Video Tutorial & Cơ chế kỹ thuật nguyên mẫu
   - 0.2. Góc nhìn Senior Mentor: Phản biện kỹ thuật & Tối ưu hóa Clean Architecture (Anti-Sycophancy)
2. **[TỔNG QUAN LUỒNG KỊCH BẢN & CỜ CỐT TRUYỆN (STORY FLAGS FLOW)](#tổng-quan-luồn-kịch-bản--cờ-cốt-truyện-story-flags-flow)**
3. **[MODULE 1: CHUẨN HÓA LỚP CHA ĐỒ VẬT CẦM TAY (`HoldableItemBase.cs`)](#module-1-chuẩn-hóa-lớp-cha-đồ-vật-cầm-tay-holdableitembasecs)**
4. **[MODULE 2: HỆ THỐNG VIỆC NHÀ, NẤU ĂN & CỐC CÀ PHÊ NÓNG](#module-2-hệ-thống-việc-nhà-nấu-ăn--cốc-cà-phê-nóng)**
   - 2.1. Mở Vali Đồ Đạc ([`LuggageInteractable.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/LuggageInteractable.cs))
   - 2.2. Chiếc Ấm Đun Nước ([`KettleItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/KettleItem.cs))
   - 2.3. Bồn Rửa Bát Lấy Nước ([`WaterSinkInteractable.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/WaterSinkInteractable.cs))
   - 2.4. Bếp Ga Đun Nước ([`GasStoveController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/GasStoveController.cs))
   - 2.5. Bát Mì Tôm Ăn Từng Miếng & Twist Hồi 2 ([`NoodleBowlController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/NoodleBowlController.cs)) - *Nâng cấp Progression Bites từ BatPan Video 3*
   - 2.6. Cốc Cà Phê Nóng Cầm Tay First-Person ([`CoffeeCupItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs)) - *Kế thừa Đa giao diện từ BatPan Video 4*
5. **[MODULE 3: HỆ THỐNG TIVI CRT ĐA KÊNH & VIDEO PLAYER (`TVController.cs`)](#module-3-hệ-thống-tivi-crt-đa-kênh--video-player-tvcontrollercs)**
   - 3.1. Phân tích kỹ thuật Tivi CRT từ Video 5 của BatPan
   - 3.2. Form Skeleton nâng cấp cho [`TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs)
6. **[MODULE 4: NÂNG CẤP SẤM SÉT & HỆ THỐNG CUTSCENE](#module-4-nâng-cấp-sấm-sét--hệ-thống-cutscene)**
   - 4.1. Nâng cấp Sấm Sét 2 Nhịp & Audio Ducking ([`ThunderEffect.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/ThunderEffect.cs))
   - 4.2. Trạng thái Khóa Điều Khiển ([`PlayerCutsceneState.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Player/PlayerState/PlayerCutsceneState.cs))
   - 4.3. Bộ Điều Khiển Cutscene ([`ICutsceneService.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/ICutsceneService.cs) & [`CutsceneController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/CutsceneController.cs))
   - 4.4. Trigger Cửa Ban Công ([`BalconyCutsceneTrigger.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/BalconyCutsceneTrigger.cs))
7. **[MODULE 5: HƯỚNG DẪN DỰNG CUTSCENE BAN CÔNG TRÊN TIMELINE (STEP-BY-STEP)](#module-5-hướng-dẫn-dựng-cutscene-ban-công-trên-timeline-step-by-step)**
8. **[MODULE 6: NÂNG CẤP GIAO DIỆN ĐIỆN THOẠI ĐA DANH BẠ (`PhoneMenu`)](#module-6-nâng-cấp-giao-diện-điện-thoại-đa-danh-bạ-phonemenu)**
9. **[MODULE 7: HỆ THỐNG DANH SÁCH VIỆC NHÀ ĐỜI THƯỜNG (`TodoListUI.cs`)](#module-7-hệ-thống-danh-sách-việc-nhà-đời-thường-todolistuics)** - *Cơ chế Signature To-Do List từ BatPan Video 1*
10. **[MODULE 8: CHECKLIST KẾT NỐI TRONG UNITY EDITOR CHO HỒI 1](#module-8-checklist-kết-nối-trong-unity-editor-cho-hồi-1)**
11. **[MODULE 9: MOCK INTERVIEW CHUẨN BỊ NGHIỆM THU TUẦN 11 - 12](#module-9-mock-interview-chuẩn-bị-nghiệm-thu-tuần-11---12)**

---

## PHẦN 0: ĐỐI CHIẾU 5 VIDEO CỦA ANH BATPAN & BẢN ĐỒ CHUYỂN HÓA SANG SOLID

### 0.1. Danh mục 5 Video Tutorial & Cơ chế kỹ thuật nguyên mẫu

5 video tutorial do YouTuber **BatPan** (kênh chuyên tái hiện các cơ chế cốt lõi của *Fears to Fathom*) cung cấp các khối xây dựng nền tảng cho nhịp sinh hoạt đời thường:

| # | Tên Video & Đường Dẫn | Chủ Đề Kỹ Thuật Chính (BatPan) | Ứng Dụng Trong Dự Án GUT FEELING |
|---|----------------------|--------------------------------|----------------------------------|
| **1** | [Build Fears to Fathom Food System in Unity – (Part 1)](https://youtu.be/EcQPcTblr4g) | Lắp ráp nguyên liệu (`Pan`, `Eggs`, `Salt`, `Pepper`), UI Checklist trực quan hiển thị số lượng nguyên liệu, ràng buộc cầm nắm 1 món (`HandIsFull`). | Giao diện Danh sách Việc nhà ([`TodoListUI.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/UI/TodoListUI.cs)), quy trình lấy nước, đặt ấm vào bếp ga, chế nước sôi vào bát mì. |
| **2** | [Build Fears to Fathom Food System in Unity – (Part 2)](https://youtu.be/_GP4rlbYv48) | Hệ thống kho đồ và tương tác (`F2FMakeFoodInteractionSystem.cs`, `F2FMakeFoodInventory.cs`), kích hoạt/ẩn mesh nguyên liệu trong chảo (`BrokenEggs_GO[]`), ngọn lửa bếp và hẹn giờ đun sôi. | Lớp cha [`HoldableItemBase`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/HoldableItemBase.cs), cơ chế [`ItemSocket`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs) đặt ấm lên bếp ga, hiệu ứng lửa và khói nước reo sôi ([`GasStoveController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/GasStoveController.cs)). |
| **3** | [Build Fears to Fathom Food System in Unity (Part 3)](https://youtu.be/8XqICLBtdzE) | Ăn từng miếng pizza (`F2FEatingSystem.cs`), mảng deactive mesh miếng (`pizzapieces[]`), âm thanh nhai (`munch-sound-effect.mp3`), khóa tương tác theo thời gian nhai (`CanInteract` coroutine delay). | Cơ chế ăn mì tôm theo nhịp (Progression Bites mảng `biteProgressMeshes[]`), âm thanh húp mì, cooldown chống spam `chewCooldown`, và dừng lại ở `HalfEaten` để chừa nửa bát cho cú twist Hồi 2! |
| **4** | [Fears to Fathom Coffee Drinking Mechanic in Unity](https://youtu.be/aXLhbJdACr8) | Cầm cốc uống cà phê góc nhìn FPS (`F2FDrinkCoffee.cs`), Animator kích hoạt clip uống (`C = 1, 2`), mực nước/texture cà phê tụt dần (`liquidMeshTransform.localScale.y`), khói bốc nghi ngút, âm thanh uống (`drink coffee.mp3`). | Chiếc cốc cà phê nóng của Nam ([`CoffeeCupItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs)), cầm giữ bằng [`IHoldInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldInteractable.cs) hoặc click bằng [`IAltUsable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IAltUsable.cs) nhấp sưởi ấm trong đêm bão; sang Hồi 2 bị kẻ ký sinh dời vị trí sang bàn bếp! |
| **5** | [Fears to Fathom TV](https://youtu.be/MxeC85f4Tu8) | Bật/tắt Tivi, phát video qua `VideoPlayer` / `RenderTexture`, đổi Material màn hình CRT, đèn hắt sáng nhấp nháy theo ánh sáng video (Screen Glow Point Light), âm thanh rè radio (`tvStaticAudio`). | Hệ thống Tivi CRT đa kênh ([`TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs)): Kênh tin bão Hồi 1 $\rightarrow$ Màn hình sập nguồn $\rightarrow$ Kênh Camera giấu kín sau tranh ở Hồi 2! Dynamic screen glow light flicker bằng Perlin Noise! |

---

### 0.2. Góc nhìn Senior Mentor: Phản biện kỹ thuật & Tối ưu hóa Clean Architecture (Anti-Sycophancy)

Khi học hỏi từ các tutorial YouTube (kể cả những tutorial rất trực quan của BatPan), một lập trình viên có tư duy cao cấp **không bao giờ sao chép nguyên xi mã nguồn**. Hãy cùng mổ xẻ những điểm hạn chế (Code Smells) trong mã nguồn gốc của BatPan và xem cách dự án GUT FEELING nâng cấp nó lên chuẩn **SOLID & Clean Architecture**:

#### 1. Vi phạm Nguyên lý Trách nhiệm Đơn lẻ (SRP) & Đóng/Mở (OCP):
* **Trong code của BatPan (`F2FMakeFoodInteractionSystem.cs`):** Toàn bộ logic bắn Raycast, đọc Input phím chuột, kiểm tra Tag chuỗi (`hit.collider.CompareTag("Eggs")`, `"Salt"`, `"Pepper"`, `"Pan"`), bật tắt UI Canvas và trừ đồ kho đều bị dồn vào một hàm `Update()` dài hơn 150 dòng.
* **Hậu quả tiêu cực:** Khi bạn muốn thêm một nguyên liệu mới (ví dụ: "Gói gia vị mì" hoặc "Hành lá"), bạn bắt buộc phải sửa file script này, viết thêm một nhánh `else if (hit.collider.CompareTag(...))` khổng lồ, và khai báo thêm các biến boolean lẻ tẻ vào `F2FMakeFoodInventory`.
* **Cách GUT FEELING giải quyết:**
  - Tách rời hoàn toàn bộ phận phát hiện tương tác: [`PlayerInteraction.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Player/PlayerInteraction.cs) chỉ giao tiếp qua hợp đồng [`IInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IInteractable.cs).
  - Tái sử dụng lớp cơ sở [`HoldableItemBase.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/HoldableItemBase.cs) và [`ItemSocket.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs). Muốn thêm ấm nước, cốc cà phê, chìa khóa hay cầu chì, ta chỉ cần tạo một lớp con kế thừa mà không sửa một dòng code nào trong hệ thống tương tác trung tâm (**Tuân thủ triệt để OCP**).

#### 2. Vi phạm Nguyên lý Đảo ngược Phụ thuộc (DIP) & Ghép nối cứng (Tight Coupling):
* **Trong code của BatPan:** Script tương tác tham chiếu trực tiếp đến từng GameObject cụ thể trong Inspector (`BrokenEggs_GO[0]`, `PepperToggle`, `WholeCanvasPack_GO`).
* **Cách GUT FEELING giải quyết:** Giao tiếp lỏng (Decoupled) qua [`StoryEventBus`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Story/StoryEventBus.cs). Khi nước sôi, bếp ga chỉ phát ra sự kiện `StoryFlag.WaterBoiling`. Khi ăn mì xong, bát mì chỉ bắn cờ `StoryFlag.NoodleCooked` và `RequestSMS(nemWarningSMS)`. Không hệ thống nào cần biết cấu trúc nội bộ của hệ thống kia!

#### 3. Rủi ro Hiệu năng (CPU / GPU & GC Allocation):
* **Trong code của BatPan:**
  - Gọi `Camera.main` trong mỗi frame `Update()`. (Dù các phiên bản Unity mới đã cache nội bộ, việc phụ thuộc vào tag tìm kiếm vẫn là thói quen xấu).
  - Bật tắt Canvas liên tục `WholeCanvasPack_GO.SetActive(false)` trong các nhánh `else` của Raycast gây lãng phí CPU và buộc Canvas phải Re-batch lại hình học.
  - Sử dụng `yield return new WaitForSeconds(1.5f)` trực tiếp trong coroutine khiến bộ gom rác (Garbage Collector) phải cấp phát đối tượng mới mỗi lần người chơi nhấp một ngụm nước hoặc ăn một miếng bánh.
* **Cách GUT FEELING giải quyết:**
  - Cache sẵn camera và các giá trị `WaitForSeconds`.
  - Quản lý giao diện dựa trên sự kiện (Event-driven UI) thay vì bật tắt liên tục trong vòng lặp mỗi frame.

#### 4. Phản biện Lỗi Lệch Trạng Thái Socket (ItemSocket Removal Desync):
* **Trong code nguyên mẫu:** Khi vật phẩm được đặt vào socket qua `placeableItem.OnPlace(attachPoint)`, socket lưu tham chiếu nội bộ (`currentItem`). Tuy nhiên, khi người chơi nhìn vào vật phẩm đó và bấm [E] nhặt lại lên tay (`Equip`), chỉ có vật phẩm chuyển `parent` về bàn tay, còn Socket hoàn toàn KHÔNG BIẾT vật phẩm đã bị nhặt đi! Hậu quả là `HasItem` bị kẹt ở `true`, ngăn người chơi đặt đồ khác vào, và sự kiện `OnItemRemoved` không bao giờ được bắn.
* **Cách GUT FEELING khắc phục:** Kiểm tra tính hợp lệ của vật phẩm trong Socket: `public bool HasItem => currentItem != null && !currentItem.IsHeld;`. Khi vật phẩm được nhặt lên tay (`OnEquip`), nó tự động xóa trạng thái socket để giải phóng khay cắm.

#### 5. Phản biện Kiến trúc Input: Tách bạch Bấm Giữ (`IHoldInteractable`) vs Bấm Tức thì (`IAltUsable`):
* **Vấn đề trong Input System:** Khi người chơi cầm cốc cà phê trên tay và bấm Chuột Phải (`AltUse`), nếu script điều khiển Player chỉ chuyển tiếp đến `StartHoldHand(player)` (vốn chỉ nhận `IHoldInteractable`), các vật phẩm chỉ triển khai `IAltUsable` đơn thuần sẽ bị liệt hoàn toàn!
* **Giải pháp Interface Segregation Principle (ISP):** Cốc cà phê [`CoffeeCupItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs) triển khai đồng thời cả `IHoldInteractable` (cho trải nghiệm giữ chuột nhấp ngụm từ từ có progress bar chân thực kiểu cinematic) lẫn `IAltUsable` (cho phép click chuột tức thì qua coroutine ngầm). Đảm bảo tính linh hoạt tối đa cho Gameplay.

---

## TỔNG QUAN LUỒNG KỊCH BẢN & CỜ CỐT TRUYỆN (STORY FLAGS FLOW)

```text
[Vào Căn Hộ: GameStarted]
       │
       ├──► 1. Giữ [E] mở khóa Vali (LuggageInteractable) 
       │         └──► Bật cờ: StoryFlag.LuggageOpened (Lộ ra Chìa khóa, Bát mì, Gói cà phê)
       │
       ├──► 2. Nhặt Chìa khóa đặt lên đĩa ở bàn (ItemSocket: "RoomKey")
       │         └──► Bật cờ: StoryFlag.KeyPlacedOnTable
       │
       ├──► 3. Bấm [E] bật Tivi xem dự báo bão (TVController + VideoPlayer CRT)
       │         └──► Bật cờ: StoryFlag.TVNewsWatched + Độc thoại của Nam
       │
       ├──► 4. Chuỗi pha đồ nóng & nấu mì tôm đêm mưa:
       │         ├── Bấm [E] vào Ấm siêu tốc cũ (BrokenKettleInteractable) -> Tạch tạch chập đế điện
       │         │     └──► Độc thoại Nam: "Ấm siêu tốc cháy đế rồi... Phải lấy chiếc nồi nhỏ đun trên bếp từ Arasapha thôi."
       │         ├── Nhặt Chiếc Nồi Inox (CookingPotItem) -> Bấm [E] vào Bồn rửa (WaterSink) hứng nước
       │         ├── Đặt Nồi lên Bếp từ Arasapha (ItemSocket: "CookingPot") -> Bấm [E] bật bếp đun sôi
       │         │     └──► Màn LED 2000W sáng, mâm nhiệt đỏ rực, nước sôi ùng ục -> Bật cờ: StoryFlag.WaterBoiling
       │         ├── Cầm Cốc Cà Phê (CoffeeCupItem) -> Bấm Chuột Phải / [E] nhấp ngụm sưởi ấm (BatPan Video 4)
       │         │     └──► Mực nước vơi dần -> Đặt cốc lên bàn làm việc
       │         └── Cầm Nồi nước sôi chế vào Cốc mì Handy Hảo Hảo (NoodleBowlController) -> Nắp bóc hé, mì nở chín
       │               └──► Ăn 2 miếng chừa lại nửa bát! -> Bật cờ: StoryFlag.NoodleCooked + Nhận SMS cảnh báo của Nem
       │
       └──► 5. Ra kéo cửa kính Ban công (BalconyCutsceneTrigger):
                 ├── Yêu cầu đủ cờ: NoodleCooked && TVNewsWatched
                 ├── Chuyển Player sang PlayerCutsceneState (Khóa di chuyển)
                 ├── Chạy Timeline: Blend Cinemachine xuống sân mưa -> Thấy kẻ mặc áo mưa nhìn lên
                 ├── Signal nổ ThunderEffect (Audio Ducking tắt tiếng mưa 0.3s -> Chớp đèn kép + Sấm giáng + Rung lắc)
                 ├── Kẻ mặc áo mưa biến mất tức thì vào frame chớp sáng -> Blend trả lại góc nhìn FPS
                 └──► Bật cờ: StoryFlag.SeenStrangerInRain (Khép lại Hồi 1, chuẩn bị cúp cầu dao Hồi 2!)
```

---

## MODULE 1: CHUẨN HÓA LỚP CHA ĐỒ VẬT CẦM TAY (`HoldableItemBase.cs`)

Lớp cơ sở kế thừa cả [`IInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IInteractable.cs), [`IHoldable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldable.cs), và [`IPlaceable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IPlaceable.cs).

### File: [`Assets/_Project/Scripts/Interaction/HoldableItemBase.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/HoldableItemBase.cs)
```csharp
using UnityEngine;

/// <summary>
/// Lớp cơ sở trừu tượng (Abstract Base Class) cho mọi vật phẩm có thể nhặt lên tay (IHoldable)
/// và đặt vào vị trí chỉ định (IPlaceable).
/// Áp dụng nguyên lý OCP & DRY: Tái sử dụng logic Rigidbody, Collider và Parenting cho Ấm nước, Cốc nước, Chìa khóa, Cầu chì...
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public abstract class HoldableItemBase : MonoBehaviour, IInteractable, IHoldable, IPlaceable
{
    [Header("Base Item Settings")]
    [SerializeField] protected string itemName = "Vật phẩm";
    [SerializeField] protected string placeableID = "DefaultID";
    [SerializeField] protected float throwForce = 5f;

    protected Rigidbody rb;
    protected Collider col;
    protected bool isHeld;
    protected Vector3 originalScale;

    public string PlaceableID => placeableID;
    public bool IsHeld => isHeld;
    public virtual string InteractionPrompt => isHeld ? string.Empty : $"[E] Nhặt {itemName}";

    protected virtual void Awake()
    {
        // TODO:
        // 1. Lấy tham chiếu Rigidbody (rb) và Collider (col) bằng GetComponent
        // 2. Lưu lại transform.localScale ban đầu vào biến originalScale
    }

    public virtual void Interact(Player player)
    {
        // TODO:
        // 1. Nếu đang cầm trên tay (isHeld == true) thì thoát hàm
        // 2. Gọi player.hand.Equip(this) để đưa vật phẩm lên tay người chơi
    }

    public virtual void OnEquip(Transform handSlot)
    {
        // TODO:
        // 1. Bật cờ isHeld = true
        // 2. Gắn vật phẩm làm con của handSlot (SetParent), đưa localPosition về zero, localRotation về identity
        // 3. Khôi phục localScale = originalScale
        // 4. Bật rb.isKinematic = true và tắt col.enabled = false để không va chạm người chơi
    }

    public virtual void OnPlace(Transform socketPoint)
    {
        // TODO:
        // 1. Đặt cờ isHeld = false
        // 2. Gắn vật phẩm làm con của socketPoint, đặt localPosition = Vector3.zero, localRotation = Quaternion.identity
        // 3. Khôi phục localScale = originalScale
        // 4. Giữ rb.isKinematic = true nhưng BẬT LẠI col.enabled = true để người chơi có thể nhặt lại khi cần
    }

    public virtual void OnDrop(Vector3 throwDirection)
    {
        // TODO:
        // 1. Đặt cờ isHeld = false và tháo khỏi tay (transform.SetParent(null))
        // 2. Khôi phục localScale = originalScale
        // 3. Bật lại vật lý: rb.isKinematic = false, col.enabled = true
        // 4. Tác dụng lực ném: rb.AddForce(throwDirection * throwForce, ForceMode.Impulse)
    }

    public virtual void OnUse()
    {
        // TODO (Tùy chọn ở lớp con): Ghi đè nếu vật phẩm có hành động khi bấm Chuột Trái
    }

    public virtual void OnSway(float lookDelta)
    {
        // TODO (Tùy chọn ở lớp con): Ghi đè nếu muốn hiệu ứng đung đưa theo góc nhìn chuột
    }
}
```

---

## MODULE 2: HỆ THỐNG VIỆC NHÀ, NẤU ĂN & CỐC CÀ PHÊ NÓNG

### 2.1. Mở Vali Đồ Đạc ([`LuggageInteractable.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/LuggageInteractable.cs))
Sử dụng hợp đồng [`IHoldInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldInteractable.cs) (giữ phím `E` để kéo khóa nắp vali).

```csharp
using System.Collections;
using UnityEngine;

public class LuggageInteractable : MonoBehaviour, IHoldInteractable
{
    [Header("Luggage Settings")]
    [SerializeField] private float openDuration = 2.0f;
    [SerializeField] private Transform lidTransform;
    [SerializeField] private float openAngle = -110f;
    [SerializeField] private GameObject[] itemsInside; // Chìa khóa, Bát mì, Cốc cà phê
    [SerializeField] private AudioSource zipperSound;

    [Header("Story Event")]
    [SerializeField] private DialogueData afterOpenDialogue;

    private bool isOpened = false;

    public float HoldDuration => openDuration;
    public string ActionPrompt => "Đang kéo khóa Vali...";
    public bool CanHold => !isOpened;

    public void OnHoldStart()
    {
        // TODO: Phát âm thanh kéo khóa zipperSound
    }

    public void OnHoldProgress(float normalizedProgress) { }

    public void OnHoldCancel()
    {
        // TODO: Dừng âm thanh kéo khóa zipperSound
    }

    public void OnHoldComplete(Player player)
    {
        if (isOpened) return;
        isOpened = true;

        // TODO:
        // 1. Chạy Coroutine xoay nắp vali (lidTransform) mở ra góc openAngle
        // 2. Bật Active cho các vật phẩm bên trong (itemsInside)
        // 3. Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.LuggageOpened, true)
        // 4. Bắn độc thoại afterOpenDialogue ("Để chìa khóa lên bàn đã, đói bụng quá...")
    }
}
```

---

### 2.2. Ấm Siêu Tốc Hỏng Nguồn & Chiếc Nồi Inox Đun Nước

> [!NOTE]
> **Plot Twist & Ý Nghĩa Chiếc Bếp Từ Arasapha:**
> Ban đầu căn phòng trọ có chiếc Ấm siêu tốc cũ để trên bệ bếp. Khi Nam bấm thử công tắc, ấm phát tiếng tạch tạch chập đế điện và không hoạt động. Nam độc thoại: *"Chết tiệt, ấm siêu tốc của chủ nhà bị cháy rơ-le đế rồi... May mà trên kệ có chiếc bếp từ Arasapha với cái nồi inox nhỏ. Phải dùng nồi đun nước sôi vậy!"*
> Chi tiết này vừa tăng tính đời thường chân thực của sinh viên ở trọ, vừa tạo ra lý do cốt truyện bắt buộc người chơi phải tương tác với chiếc **Bếp từ ARASAPHA**!

#### 2.2A. Chiếc Ấm Siêu Tốc Cũ Bị Hỏng (`BrokenKettleInteractable.cs`)
Tương tác tại chỗ ([`IInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IInteractable.cs)), phát tiếng chập điện và kích hoạt độc thoại gợi ý người chơi đi lấy chiếc nồi.

```csharp
using UnityEngine;

/// <summary>
/// Chiếc ấm siêu tốc cũ để sẵn trên bệ bếp nhưng đã bị cháy đế điện.
/// Khi tương tác, phát tiếng tạch tạch chập nguồn và kích hoạt độc thoại gợi ý Nam dùng nồi đun trên bếp từ.
/// </summary>
public class BrokenKettleInteractable : MonoBehaviour, IInteractable
{
    [Header("Audio & Effects")]
    [SerializeField] private AudioSource sparkAudio;
    [SerializeField] private ParticleSystem sparkVFX;

    [Header("Dialogue")]
    [SerializeField] private DialogueData brokenDialogue;

    private bool hasInteracted;

    public string InteractionPrompt => "[E] Bật ấm đun nước";

    public void Interact(Player player)
    {
        // 1. Phát âm thanh tạch tạch chập điện và hiệu ứng tia lửa nhỏ (nếu có)
        if (sparkAudio != null) sparkAudio.Play();
        if (sparkVFX != null) sparkVFX.Play();

        // 2. Bắn độc thoại Nam phát hiện ấm cháy nguồn, gợi ý dùng nồi inox trên bếp từ Arasapha
        if (brokenDialogue != null)
        {
            StoryEventBus.Instance.RequestDialogue(brokenDialogue);
        }

        hasInteracted = true;
    }
}
```

#### 2.2B. Chiếc Nồi Inox Nấu Mì ([`CookingPotItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CookingPotItem.cs))
Kế thừa [`HoldableItemBase`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/HoldableItemBase.cs), `placeableID = "CookingPot"`, quản lý hai trạng thái `HasWater` và `IsBoiled`.

```csharp
using UnityEngine;

/// <summary>
/// Chiếc nồi/quánh inox nhỏ Nam lấy để đun nước trên bếp từ Arasapha sau khi ấm siêu tốc bị hỏng.
/// Kế thừa từ HoldableItemBase (LSP/OCP).
/// </summary>
public class CookingPotItem : HoldableItemBase
{
    [Header("Cooking Pot Specifics")]
    [SerializeField] private ParticleSystem steamParticles;
    [SerializeField] private AudioSource waterSloshAudio;
    [SerializeField] private GameObject waterInsideMesh; // Mesh mặt nước bên trong nồi

    public bool HasWater { get; private set; }
    public bool IsBoiled { get; private set; }

    public override string InteractionPrompt
    {
        get
        {
            if (isHeld) return string.Empty;
            if (IsBoiled) return "[E] Cầm Nồi nước sôi";
            if (HasWater) return "[E] Cầm Nồi nước (Chưa đun)";
            return "[E] Nhặt Nồi inox";
        }
    }

    public void FillWater()
    {
        // TODO:
        // 1. Gán HasWater = true
        // 2. Bật hiển thị waterInsideMesh (nếu có)
        // 3. Phát tiếng nước sánh waterSloshAudio
    }

    public void SetBoiled()
    {
        // TODO:
        // 1. Gán IsBoiled = true
        // 2. Bật khói nghi ngút: steamParticles.Play()
    }
}
```

---

### 2.3. Bồn Rửa Bát Hứng Nước Vào Nồi ([`WaterSinkInteractable.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/WaterSinkInteractable.cs))
Kiểm tra vật đang cầm trên tay (`player.hand.CurrentItemHold`) có phải là [`CookingPotItem`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CookingPotItem.cs) (hoặc [`KettleItem`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/KettleItem.cs)) hay không bằng C# Pattern Matching.

```csharp
using UnityEngine;

public class WaterSinkInteractable : MonoBehaviour, IInteractable
{
    [Header("Sink Components")]
    [SerializeField] private AudioSource waterTapAudio;
    [SerializeField] private ParticleSystem waterStreamVFX;

    [Header("Story Feedback")]
    [SerializeField] private DialogueData needPotDialogue; // "Cần tìm chiếc nồi để hứng nước..."
    [SerializeField] private DialogueData alreadyFilledDialogue;

    public string InteractionPrompt => "[E] Mở vòi lấy nước";

    public void Interact(Player player)
    {
        // TODO:
        // 1. Kiểm tra nếu player.hand.CurrentItemHold is CookingPotItem pot:
        //    - Nếu !pot.HasWater:
        //      + Gọi pot.FillWater()
        //      + Bật waterStreamVFX và phát tiếng waterTapAudio
        //    - Nếu đã có nước: Bắn thoại alreadyFilledDialogue
        // 2. Tương tự kiểm tra nếu là KettleItem kettle -> kettle.FillWater()
        // 3. Nếu không cầm nồi/ấm: Bắn thoại needPotDialogue ("Cần tìm chiếc nồi để hứng nước...")
    }
}
```

---

### 2.4. Bếp Từ ARASAPHA Đun Nước ([`GasStoveController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/GasStoveController.cs))
Phối hợp cùng [`ItemSocket`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/ItemSocket.cs) (`requiredItemTag = "CookingPot"` hoặc `"Kettle"`), mâm nhiệt hồng ngoại phát sáng `Induction_GlowVFX`.

```csharp
using System.Collections;
using UnityEngine;

public class GasStoveController : MonoBehaviour, IInteractable
{
    [Header("Stove References")]
    [SerializeField] private ItemSocket potSocket; // Đổi hoặc gắn tag "CookingPot" / "Kettle"
    [SerializeField] private GameObject inductionGlowVFX; // Vòng nhiệt đỏ rực của bếp từ Arasapha
    [SerializeField] private AudioSource buttonBeepAudio; // Tiếng tít cảm ứng khi bật bếp
    [SerializeField] private AudioSource boilingAudio;
    [SerializeField] private float boilDuration = 5.0f;

    [Header("Story & Feedback")]
    [SerializeField] private DialogueData noPotDialogue; // "Chưa đặt nồi lên bếp!"
    [SerializeField] private DialogueData emptyPotDialogue; // "Nồi chưa có nước!"
    [SerializeField] private DialogueData waterBoiledDialogue;

    private bool isBoiling;
    private CookingPotItem placedPot;

    public bool IsBoiling => isBoiling;
    public string InteractionPrompt => isBoiling ? "Đang đun nước..." : "[E] Bật bếp từ đun nước";

    public void Interact(Player player)
    {
        if (isBoiling) return;

        // TODO:
        // 1. Lấy placedPot = potSocket.GetComponentInChildren<CookingPotItem>()
        // 2. Nếu placedPot == null -> Bắn thoại noPotDialogue ("Cần đặt nồi lên bếp trước")
        // 3. Nếu !placedPot.HasWater -> Bắn thoại emptyPotDialogue ("Nồi chưa có nước!")
        // 4. Nếu placedPot.IsBoiled -> Thoát hàm (nước đã sôi)
        // 5. Đủ điều kiện -> StartCoroutine(BoilRoutine())
    }

    private IEnumerator BoilRoutine()
    {
        isBoiling = true;

        // TODO:
        // 1. Bật inductionGlowVFX (vòng nhiệt đỏ rực), phát tiếng tít cảm ứng buttonBeepAudio và tiếng boilingAudio
        // 2. Khóa Collider của placedPot để người chơi không nhấc nồi ra khi đang đun dở
        // 3. Chờ: yield return new WaitForSeconds(boilDuration)
        // 4. Tắt inductionGlowVFX, tắt âm thanh đun, gọi placedPot.SetBoiled(), bật lại Collider của nồi
        // 5. Bật cờ StoryEventBus.Instance.SetFlag(StoryFlag.WaterBoiling, true)
        // 6. Bắn độc thoại waterBoiledDialogue ("Nước trong nồi sôi sùng sục rồi, rót vào cốc mì Handy Hảo Hảo thôi!")
        // 7. isBoiling = false
        yield return null;
    }
}
```

---

### 2.5. Bát Mì Tôm Ăn Từng Miếng & Twist Hồi 2 ([`NoodleBowlController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/NoodleBowlController.cs))
*Nâng cấp từ Video 1, 2, 3 của BatPan:* Thay vì chỉ là 1 cú click đơn điệu, BatPan chia nhỏ món ăn thành từng miếng (`SliceEaten`, `pizzapieces[]`). Trong GUT FEELING, Nam gắp ăn 2 miếng mì nóng hổi qua mảng `biteProgressMeshes[]`, có thời gian nhai khóa tương tác (`chewCooldown`), nhưng rồi **bắt buộc dừng lại chừa lại nửa bát** (`HalfEaten`) do chuông tin nhắn của Nem rung lên và tiếng gió đập mạnh ngoài cửa sổ!

```csharp
#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Quản lý Bát mì tôm trong Hồi 1 & Twist Kẻ ký sinh ăn vụng ở Hồi 2.
/// Kế thừa kỹ thuật ăn từng miếng từ BatPan Video 3 (F2FEatingSystem: progression bites,
/// lockout cooldown, âm thanh nhai/húp theo nhịp).
/// Triển khai cả IInteractable (bấm [E] chế nước sôi) và IHoldInteractable (giữ [E] gắp từng miếng mì).
/// </summary>
public class NoodleBowlController : MonoBehaviour, IInteractable, IHoldInteractable
{
    public enum NoodleState
    {
        Dry,
        PouredHotWater,
        EatingInProgress,
        HalfEaten,
        EmptyByStalker
    }

    [Header("Visual Meshes (Bật/Tắt theo tiến trình ăn - BatPan Video 3)")]
    [SerializeField] private GameObject dryNoodleMesh;
    [SerializeField] private GameObject cookedNoodleMesh;
    [SerializeField] private GameObject[] biteProgressMeshes; // Mảng mesh các mức vơi của bát mì (BatPan pizzapieces[])
    [SerializeField] private GameObject halfEatenMesh;
    [SerializeField] private GameObject emptyStalkerMesh;
    [SerializeField] private GameObject mysteriousHalfDrunkCup; // Cốc nước lạ xuất hiện cạnh bát mì ở Hồi 2!
    [SerializeField] private ParticleSystem hotSteamVFX;

    [Header("Audio")]
    [SerializeField] private AudioSource pourWaterAudio;
    [SerializeField] private AudioSource slurpEatAudio;

    [Header("Eating Settings (BatPan Video 3)")]
    [SerializeField] private float eatHoldDuration = 2.5f;
    [SerializeField] private float chewCooldown = 1.0f; // Khóa tương tác giữa các miếng để tránh spam (CanInteract delay)
    [SerializeField] private int maxAct1Bites = 2;       // Nam chỉ gắp 2 miếng rồi chừa lại nửa bát (HalfEaten)

    [Header("Story & SMS")]
    [SerializeField] private DialogueData needBoiledWaterDialogue;
    [SerializeField] private DialogueData firstBiteDialogue;
    [SerializeField] private DialogueData afterEatDialogue;
    [SerializeField] private SMSData nemWarningSMS;
    [SerializeField] private DialogueData stalkerAteNoodleDialogue;

    private NoodleState currentState = NoodleState.Dry;
    private int currentBite = 0;
    private bool isChewingLockout = false;
    private int stateChangedFrame = -1;

    public NoodleState CurrentState => currentState;
    public int CurrentBite => currentBite;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // - Khi Dry: "[E] Chế nước sôi vào mì"
            // - Khi PouredHotWater hoặc EatingInProgress: isChewingLockout ? "Đang nhai..." : "Giữ [E] Gắp miếng mì"
            // - Khi EmptyByStalker (Hồi 2): "[E] Kiểm tra bát mì"
            // - Các trạng thái khác: string.Empty
            if (currentState == NoodleState.Dry) return "[E] Chế nước sôi vào mì";
            if (currentState == NoodleState.PouredHotWater || currentState == NoodleState.EatingInProgress)
                return isChewingLockout ? "Đang nhai..." : "Giữ [E] Gắp miếng mì";
            if (currentState == NoodleState.EmptyByStalker) return "[E] Kiểm tra bát mì";
            return string.Empty;
        }
    }

    private void Start()
    {
        // TODO: Đăng ký lắng nghe StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged
    }

    private void OnDestroy()
    {
        // TODO: Hủy đăng ký StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged
    }

    public void Interact(Player player)
    {
        // TODO:
        // TRƯỜNG HỢP 1: Dry -> Kiểm tra player.hand.CurrentItemHold is KettleItem kettle && kettle.IsBoiled
        //   + Chuyển currentState = PouredHotWater, lưu stateChangedFrame = Time.frameCount
        //   + Tắt dryNoodleMesh, bật cookedNoodleMesh, bật hotSteamVFX, phát pourWaterAudio
        // TRƯỜNG HỢP 2: EmptyByStalker -> Bật cờ StoryFlag.HalfEatenNoodleSeen và bắn thoại stalkerAteNoodleDialogue
    }

    public float HoldDuration => eatHoldDuration;
    public string ActionPrompt => $"Đang ăn miếng {currentBite + 1}...";

    public bool CanHold =>
        (currentState == NoodleState.PouredHotWater || currentState == NoodleState.EatingInProgress) &&
        !isChewingLockout &&
        Time.frameCount > stateChangedFrame;

    public void OnHoldStart()
    {
        // TODO: Phát âm thanh húp mì slurpEatAudio.Play()
    }

    public void OnHoldProgress(float normalizedProgress) { }

    public void OnHoldCancel()
    {
        // TODO: Dừng slurpEatAudio.Stop()
    }

    public void OnHoldComplete(Player player)
    {
        // TODO:
        // 1. Tăng currentBite lên 1
        // 2. Cập nhật mesh: ẩn mức cũ, bật biteProgressMeshes[currentBite - 1] (kế thừa từ BatPan pizzapieces[])
        // 3. Khởi chạy Coroutine khóa thao tác tạm thời: StartCoroutine(ChewLockoutRoutine())
        // 4. Nếu currentBite == 1:
        //    - currentState = NoodleState.EatingInProgress
        //    - Bắn độc thoại firstBiteDialogue ("Nóng hổi vừa thổi vừa ăn...")
        // 5. Nếu currentBite >= maxAct1Bites:
        //    - currentState = NoodleState.HalfEaten (Dừng lại chừa nửa bát!)
        //    - Bật halfEatenMesh
        //    - Bật cờ: StoryEventBus.Instance.SetFlag(StoryFlag.NoodleCooked, true)
        //    - Bắn độc thoại afterEatDialogue ("Mưa gió làm bát mì ấm cả người...")
        //    - Bắn tin nhắn cảnh báo của Nem: StoryEventBus.Instance.RequestSMS(nemWarningSMS)
    }

    private IEnumerator ChewLockoutRoutine()
    {
        isChewingLockout = true;
        // TODO:
        // yield return new WaitForSeconds(chewCooldown);
        // isChewingLockout = false;
        yield return null;
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO (Chuẩn bị cho Hồi 2):
        // Khi flag == StoryFlag.PowerRestored:
        // - Chuyển currentState = EmptyByStalker
        // - Tắt halfEatenMesh, bật emptyStalkerMesh
        // - Bật chiếc cốc bí ẩn: mysteriousHalfDrunkCup.SetActive(true)
    }
}
```

---

### 2.6. Cốc Cà Phê Nóng Cầm Tay First-Person ([`CoffeeCupItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs))
*Lấy cảm hứng trực tiếp từ Video 4 của BatPan (`Fears to Fathom Coffee Drinking Mechanic`):*
- Cho phép Nam cầm cốc cà phê trên tay và nhấp từng ngụm:
  - **Bằng cách Giữ Chuột Phải / Phím AltUse** (triển khai [`IHoldInteractable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IHoldInteractable.cs), hiển thị Progress Bar "Đang uống cà phê..." và lerp mực nước từ từ).
  - **Hoặc Bằng cách Bấm Click Chuột Trái / F** (triển khai [`IAltUsable`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/IAltUsable.cs) & ghi đè `OnUse()`).
- Mỗi lần nhấp ngụm: Kích hoạt Animator đưa cốc lên miệng, phát âm thanh húp nước nóng (`sipAudioClip`), hạ thấp mực nước (`liquidMeshTransform.localScale.y`), và phát tiếng nuốt (`swallowAudioClip`).
- Khi cạn cốc (`maxSips` đạt tối đa): Tắt hệ thống khói nóng (`hotSteamVFX.Stop()`).
- **Điểm thắt nút kịch bản (Narrative Hook):** Khi uống xong ngụm 1, Nam đặt cốc lên bàn máy tính. Sang Hồi 2 sau khi cúp điện rồi có lại, chiếc cốc này đã bị kẻ ký sinh dời sang bàn bếp ăn cạnh bát mì (`mysteriousHalfDrunkCup`)!

### File: [`Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Interaction/CoffeeCupItem.cs)
```csharp
#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;

/// <summary>
/// Chiếc cốc cà phê/trà nóng của Nam trong đêm mưa bão Hồi 1.
/// Lấy cảm hứng từ cơ chế uống đồ nóng của Fears to Fathom (Video 4 - BatPan).
/// Kế thừa HoldableItemBase (LSP/OCP) và triển khai cả IAltUsable lẫn IHoldInteractable
/// để hỗ trợ tương thích hoàn hảo với hệ thống cầm giữ đồ trên tay (StartHoldHand).
/// </summary>
public class CoffeeCupItem : HoldableItemBase, IAltUsable, IHoldInteractable
{
    [Header("Coffee Visual & Animation (BatPan Video 4)")]
    [SerializeField] private Animator mugAnimator;
    [SerializeField] private Transform liquidMeshTransform; // Bề mặt nước để hạ độ cao khi uống
    [SerializeField] private ParticleSystem hotSteamVFX;    // Khói bốc lên từ cốc nước nóng

    [Header("Audio")]
    [SerializeField] private AudioSource coffeeAudioSource;
    [SerializeField] private AudioClip sipAudioClip;
    [SerializeField] private AudioClip swallowAudioClip;

    [Header("Drinking Settings")]
    [SerializeField] private int maxSips = 2; // Uống 2 ngụm: ngụm 1 vơi nửa cốc, ngụm 2 cạn đáy
    [SerializeField] private float sipCooldown = 1.5f;

    [Header("Story Dialogues")]
    [SerializeField] private DialogueData firstSipDialogue; // "Cà phê nóng làm người tỉnh táo hẳn..."
    [SerializeField] private DialogueData emptyCupDialogue;  // "Hết sạch rồi."

    private int sipsTaken = 0;
    private bool isDrinking = false;
    private Vector3 initialLiquidScale;

    public bool IsEmpty => sipsTaken >= maxSips;
    public int SipsTaken => sipsTaken;

    public override string InteractionPrompt
    {
        get
        {
            if (isHeld) return string.Empty;
            return IsEmpty ? "[E] Nhặt Cốc rỗng" : $"[E] Nhặt Cốc {itemName}";
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (liquidMeshTransform != null)
        {
            initialLiquidScale = liquidMeshTransform.localScale;
        }
    }

    // =========================================================================
    // 1. HỢP ĐỒNG IHoldInteractable (Giữ chuột phải / phím AltUse để nhấp từng ngụm)
    // =========================================================================

    public float HoldDuration => sipCooldown;
    public string ActionPrompt => "Đang uống cà phê...";
    public bool CanHold => isHeld && !IsEmpty && !isDrinking;

    public void OnHoldStart()
    {
        // TODO:
        // 1. Bật cờ isDrinking = true
        // 2. Kích hoạt Animator: mugAnimator.SetInteger("DrinkStep", sipsTaken + 1)
        // 3. Phát tiếng húp nước nóng: coffeeAudioSource.PlayOneShot(sipAudioClip)
    }

    public void OnHoldProgress(float normalizedProgress)
    {
        // TODO:
        // Hạ dần mực nước liquidMeshTransform theo tiến trình normalizedProgress:
        // Target Y scale = Lerp từ mức hiện tại xuống mức sau ngụm uống kế tiếp.
    }

    public void OnHoldCancel()
    {
        // TODO: Khôi phục Animator về Idle, đặt isDrinking = false
    }

    public void OnHoldComplete(Player player)
    {
        // TODO:
        // 1. Tăng sipsTaken lên 1
        // 2. Phát tiếng nuốt nước: coffeeAudioSource.PlayOneShot(swallowAudioClip)
        // 3. Cập nhật cố định scale của liquidMeshTransform
        // 4. Nếu sipsTaken >= maxSips -> Tắt hotSteamVFX.Stop() (cốc đã cạn và nguội)
        // 5. Bắn độc thoại cốt truyện:
        //    - sipsTaken == 1: StoryEventBus.Instance.RequestDialogue(firstSipDialogue)
        //    - IsEmpty: StoryEventBus.Instance.RequestDialogue(emptyCupDialogue)
        // 6. Đặt isDrinking = false
    }

    // =========================================================================
    // 2. HỢP ĐỒNG IAltUsable & OnUse (Hỗ trợ click chuột tức thì qua Coroutine)
    // =========================================================================

    public void OnAltUse()
    {
        if (isDrinking || IsEmpty) return;
        StartCoroutine(DrinkCoffeeRoutine());
    }

    public override void OnUse()
    {
        OnAltUse();
    }

    private IEnumerator DrinkCoffeeRoutine()
    {
        isDrinking = true;

        // TODO:
        // 1. OnHoldStart() mô phỏng: Kích hoạt mugAnimator và phát sipAudioClip
        // 2. Chờ thời gian ngụm uống: yield return new WaitForSeconds(sipCooldown)
        // 3. Gọi logic hoàn tất uống OnHoldComplete(null)
        yield return null;
    }
}
```

---

## MODULE 3: HỆ THỐNG TIVI CRT ĐA KÊNH & VIDEO PLAYER (`TVController.cs`)

### 3.1. Phân tích kỹ thuật Tivi CRT từ Video 5 của BatPan
Trong Video 5 (*Fears to Fathom TV*), BatPan trình diễn cách dựng một chiếc Tivi cổ điển:
1. **MeshRenderer & Material Index:** Mặt kính tivi là một Sub-mesh riêng (thường ở Material Index 1 hoặc 0). Khi tivi tắt, gán vật liệu kính đen tắt đèn (`offScreenMat`). Khi bật, đổi sang vật liệu phát sáng (Emission).
2. **VideoPlayer Component:** Gắn component `UnityEngine.Video.VideoPlayer` lên tivi:
   - *Render Mode:* Chọn `Render Texture` chiếu vào một `RenderTexture 512x384` (chuẩn tỷ lệ 4:3 của màn hình bóng đèn hình CRT hoài cổ).
   - *Audio Output Mode:* Chọn `Audio Source` và gán vào một chiếc `AudioSource` 3D đặt ngay tại loa của tivi (được lọc High-pass / Low-pass cho âm thanh hơi rè méo tiếng giống loa tivi cũ).
3. **Đèn hắt sáng màn hình (Dynamic Screen Glow Light):** Một chiếc `Point Light` hoặc `Spot Light` chiếu từ mặt tivi ra sàn phòng khách. Để tạo hiệu ứng chân thực như phim kinh dị, ánh sáng này phải nhấp nháy (flicker) theo từng khung hình của video.

---

### 3.2. Form Skeleton nâng cấp cho [`TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs)

### File: [`Assets/_Project/Scripts/Environment/TVController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/TVController.cs)
```csharp
#pragma warning disable CS0414, CS0649
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Quản lý Tivi CRT trong phòng khách xuyên suốt Hồi 1 (Bản tin bão) và Hồi 2 (Camera giấu kín sau tranh).
/// Học hỏi kỹ thuật VideoPlayer & Dynamic Screen Glow từ BatPan Video 5, kết hợp DIP với StoryEventBus.
/// </summary>
public class TVController : MonoBehaviour, IInteractable
{
    public enum TVChannelState
    {
        Off,
        WeatherNewsAct1,
        HiddenCameraAct2
    }

    [Header("TV Hardware & Screen")]
    [SerializeField] private MeshRenderer screenRenderer;
    [SerializeField] private int screenMaterialIndex = 0;
    [SerializeField] private Light screenGlowLight;
    [SerializeField] private AudioSource tvStaticAudio;
    [SerializeField] private AudioSource newsBroadcastAudio;

    [Header("Video Player Setup (BatPan Video 5)")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip weatherNewsClip;
    [SerializeField] private RenderTexture tvRenderTexture;

    [Header("Dynamic Screen Glow (BatPan Video 5)")]
    [SerializeField] private float minGlowIntensity = 0.6f;
    [SerializeField] private float maxGlowIntensity = 2.2f;
    [SerializeField] private float glowFlickerSpeed = 14f;

    [Header("Channel Materials")]
    [SerializeField] private Material offScreenMat;
    [SerializeField] private Material weatherNewsMat;
    [SerializeField] private Material hiddenCameraMat; // Gắn RenderTexture của Camera giấu sau bức tranh!

    [Header("Hidden Camera Reference (Act 2)")]
    [SerializeField] private Camera hiddenWallCamera;

    [Header("Story Dialogues")]
    [SerializeField] private DialogueData watchNewsDialogue;
    [SerializeField] private DialogueData creepyHiddenCamDialogue;

    private TVChannelState currentChannel = TVChannelState.Off;
    private Coroutine glowRoutine;

    public TVChannelState CurrentChannel => currentChannel;

    public string InteractionPrompt
    {
        get
        {
            // TODO:
            // - Nếu đang ở HiddenCameraAct2: "[E] Xem kỹ màn hình Tivi"
            // - Nếu đang Off: "[E] Bật Tivi"
            // - Nếu đang WeatherNewsAct1: "[E] Tắt Tivi"
            return currentChannel == TVChannelState.Off ? "[E] Bật Tivi" : "[E] Tắt Tivi";
        }
    }

    private void Start()
    {
        // TODO:
        // 1. Tắt hiddenWallCamera (hiddenWallCamera.enabled = false) ở Hồi 1 để tiết kiệm triệt để GPU
        // 2. Đăng ký StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged
    }

    private void OnDestroy()
    {
        // TODO: Hủy đăng ký StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged
    }

    public void Interact(Player player)
    {
        // TODO:
        // 1. Nếu đang mất điện (HasFlag(PowerOutageOccurred) && !HasFlag(PowerRestored)) -> Thoát hàm
        // 2. Nếu đang ở HiddenCameraAct2 (Hồi 2):
        //    - Bật cờ StoryFlag.CreepyTVBroadcastSeen = true
        //    - Bắn độc thoại creepyHiddenCamDialogue ("Góc quay này... là từ bức tranh sau lưng mình?!")
        // 3. Ở Hồi 1: Đảo trạng thái giữa Off và WeatherNewsAct1:
        //    - Nếu bật WeatherNewsAct1:
        //      + Gán screenRenderer.material = weatherNewsMat
        //      + Bật screenGlowLight.enabled = true và khởi chạy glowRoutine = StartCoroutine(DynamicScreenGlowRoutine())
        //      + Nếu có videoPlayer -> videoPlayer.clip = weatherNewsClip; videoPlayer.targetTexture = tvRenderTexture; videoPlayer.Play();
        //      + Nếu bật lần đầu: Bật cờ StoryFlag.TVNewsWatched và bắn thoại watchNewsDialogue
        //    - Nếu tắt (Off):
        //      + Gán screenRenderer.material = offScreenMat
        //      + Tắt screenGlowLight.enabled = false, dừng glowRoutine
        //      + Nếu có videoPlayer -> videoPlayer.Stop();
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO:
        // 1. Khi flag == StoryFlag.PowerOutageOccurred (Sập nguồn):
        //    - Cưỡng bức chuyển currentChannel = TVChannelState.Off, tắt đèn, dừng videoPlayer và mọi AudioSource
        // 2. Khi flag == StoryFlag.PowerRestored (Có điện lại):
        //    - Tự động chuyển currentChannel = TVChannelState.HiddenCameraAct2
        //    - Bật hiddenWallCamera.enabled = true
        //    - Gán hiddenCameraMat lên màn hình Tivi, bật screenGlowLight và phát tiếng rè tvStaticAudio
    }

    private IEnumerator DynamicScreenGlowRoutine()
    {
        // TODO:
        // Giả lập ánh sáng hắt từ màn hình CRT nhấp nháy theo nhịp video:
        // Trong khi currentChannel != TVChannelState.Off && screenGlowLight != null:
        // screenGlowLight.intensity = Mathf.Lerp(minGlowIntensity, maxGlowIntensity, Mathf.PerlinNoise(Time.time * glowFlickerSpeed, 0f));
        // yield return null;
        yield return null;
    }
}
```

---

## MODULE 4: NÂNG CẤP SẤM SÉT & HỆ THỐNG CUTSCENE

### 4.1. Nâng cấp Sấm Sét 2 Nhịp & Audio Ducking ([`ThunderEffect.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Environment/ThunderEffect.cs))
Sấm chớp thật ngoài đời không bao giờ sáng 1 nhịp rồi tắt; nó luôn có **nhịp chớp mồi (pre-flash) cực nhanh 0.04s** rồi mới đến **cú giáng sấm chính kèm chớp lóa kéo dài**. Đặc biệt, kỹ thuật điện ảnh **Audio Ducking** hạ tiếng mưa trước 0.3s tạo một khoảng lặng rợn người trước khi tiếng sấm xé toạc màn đêm.

```csharp
#pragma warning disable CS0414, CS0649
using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

/// <summary>
/// Hiệu ứng sấm sét điện ảnh trong đêm bão Hồi 1 & Hồi 2.
/// Tích hợp CinemachineImpulseSource, chớp sáng kép (Pre-flash + Main flash)
/// và Audio Ducking hạ âm lượng mưa trước khi sấm nổ.
/// </summary>
public class ThunderEffect : MonoBehaviour
{
    [Header("Impulse & Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Audio & Ducking")]
    [SerializeField] private AudioSource thunderAudioSource;
    [SerializeField] private AudioSource rainAudioSource; // Giảm âm lượng mưa để sấm nổ uy lực
    [SerializeField] private float rainDuckedVolume = 0.15f;
    [SerializeField] private float duckLeadTime = 0.3f; // Hạ tiếng mưa trước khi sấm đánh 0.3s

    [Header("Lightning Flash Setup")]
    [SerializeField] private Light lightningLight; // Đèn ngoài ban công chiếu xuyên cửa kính
    [SerializeField] private float maxIntensity = 12.0f;
    [SerializeField] private float flashDuration = 0.4f;

    private float originalRainVolume = 1.0f;

    private void Awake()
    {
        if (impulseSource == null) impulseSource = GetComponent<CinemachineImpulseSource>();
        if (lightningLight != null) lightningLight.intensity = 0f;
        if (rainAudioSource != null) originalRainVolume = rainAudioSource.volume;
    }

    [ContextMenu("Test Thunder")]
    public void TriggerThunder()
    {
        if (rainAudioSource != null) StartCoroutine(AudioDuckingRoutine());
        if (thunderAudioSource != null) thunderAudioSource.Play();
        if (impulseSource != null) impulseSource.GenerateImpulse();
        if (lightningLight != null) StartCoroutine(FlashLightRoutine());
    }

    private IEnumerator FlashLightRoutine()
    {
        // TODO: Tạo hiệu ứng chớp sét 2 nhịp chuyên nghiệp:
        // Nhịp 1 (Pre-flash): Bật intensity = maxIntensity * 0.5f -> Chờ 0.04s -> Tắt về 0 -> Chờ 0.05s
        // Nhịp 2 (Main flash): Bật intensity = maxIntensity -> Giảm dần (Lerp) về 0 theo thời gian flashDuration!
        yield return null;
    }

    private IEnumerator AudioDuckingRoutine()
    {
        // TODO:
        // 1. Hạ volume rainAudioSource xuống rainDuckedVolume
        // 2. Chờ sấm nổ: yield return new WaitForSeconds(duckLeadTime + 1.5f)
        // 3. Khôi phục dần volume rainAudioSource về originalRainVolume
        yield return null;
    }
}
```

---

### 4.2. Trạng thái Khóa Điều Khiển ([`PlayerCutsceneState.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Player/PlayerState/PlayerCutsceneState.cs))
Kế thừa từ [`PlayerState.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Player/PlayerState/PlayerState.cs), ngừng toàn bộ quán tính di chuyển và lia chuột của Nam khi Timeline đang chiếm quyền điều khiển.

```csharp
using UnityEngine;

public class PlayerCutsceneState : PlayerState
{
    public PlayerCutsceneState(Player player, StateMachine stateMachine) : base(player, stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // TODO:
        // 1. Dừng ngay lập tức di chuyển của nhân vật (player.movement.Stop())
        // 2. Ẩn tâm ngắm / Prompt tương tác trên màn hình
        // 3. Nếu đang mở điện thoại thì đóng lại
    }

    public override void Update()
    {
        base.Update();
        // Không nhận input di chuyển hay xoay camera để Timeline hoàn toàn làm chủ!
    }
}
```

---

### 4.3. Bộ Điều Khiển Cutscene ([`ICutsceneService.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/ICutsceneService.cs) & [`CutsceneController.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/CutsceneController.cs))

```csharp
using System;
using UnityEngine;
using UnityEngine.Playables;

public interface ICutsceneService
{
    bool IsPlaying { get; }
    event Action OnCutsceneStarted;
    event Action OnCutsceneEnded;
    void PlayTimeline(PlayableDirector director, Action onComplete = null);
}

public class CutsceneController : MonoBehaviour, ICutsceneService
{
    public static ICutsceneService Instance { get; private set; }

    public bool IsPlaying { get; private set; }
    public event Action OnCutsceneStarted;
    public event Action OnCutsceneEnded;

    private Action currentOnComplete;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayTimeline(PlayableDirector director, Action onComplete = null)
    {
        if (director == null || IsPlaying) return;

        IsPlaying = true;
        currentOnComplete = onComplete;
        director.stopped += OnDirectorStopped;

        OnCutsceneStarted?.Invoke();
        director.Play();
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnDirectorStopped;
        IsPlaying = false;

        OnCutsceneEnded?.Invoke();
        currentOnComplete?.Invoke();
        currentOnComplete = null;
    }
}
```

---

### 4.4. Trigger Cửa Ban Công ([`BalconyCutsceneTrigger.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Cutscenes/BalconyCutsceneTrigger.cs))

```csharp
using UnityEngine;
using UnityEngine.Playables;

public class BalconyCutsceneTrigger : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] private StoryFlag requiredFlag = StoryFlag.NoodleCooked;
    [SerializeField] private DialogueData notReadyDialogue;

    [Header("Cutscene Setup")]
    [SerializeField] private PlayableDirector balconyTimeline;
    [SerializeField] private StoryFlag flagOnComplete = StoryFlag.SeenStrangerInRain;
    [SerializeField] private DialogueData afterCutsceneDialogue;

    private bool hasPlayed = false;

    public string InteractionPrompt => hasPlayed ? string.Empty : "[E] Đóng cửa kính Ban công";

    public void Interact(Player player)
    {
        if (hasPlayed) return;

        // 1. Kiểm tra đã nấu và ăn mì xong chưa
        if (!StoryEventBus.Instance.HasFlag(requiredFlag))
        {
            StoryEventBus.Instance.RequestDialogue(notReadyDialogue);
            return;
        }

        // 2. Chạy Cutscene Ban công
        hasPlayed = true;
        CutsceneController.Instance.PlayTimeline(balconyTimeline, OnCutsceneFinished);
    }

    private void OnCutsceneFinished()
    {
        // TODO:
        // 1. Bật cờ StoryEventBus.Instance.SetFlag(flagOnComplete, true)
        // 2. Bắn độc thoại hoảng hốt của Nam: RequestDialogue(afterCutsceneDialogue)
    }
}
```

---

## MODULE 5: HƯỚNG DẪN DỰNG CUTSCENE BAN CÔNG TRÊN TIMELINE (STEP-BY-STEP)

Thực hiện đúng 5 bước dàn dựng điện ảnh trong Unity Editor:

1. **Chuẩn bị 2 Cinemachine Virtual Cameras:**
   - `VCam_PlayerFPS`: Gắn ở mắt nhân vật Nam (Priority = 10, Field of View = 75).
   - `VCam_BalconyZoom`: Đặt ở mép lan can ban công tầng 4, chúc góc nhìn xuống mặt sân mưa cạnh cột đèn đường (Priority = 0, Field of View = 60).
2. **Tạo GameObject Kẻ Mặc Áo Mưa (`StrangerInRain`):**
   - Đặt đứng im dưới cột đèn đường ngoài mưa, đầu ngửa ngược lên 80 độ nhìn chằm chằm vào cửa sổ ban công phòng Nam.
   - Mặc định để `SetActive(false)`.
3. **Kỹ thuật Audio Ducking (Khoảng Lặng Rợn Gáy):**
   - Tiếng mưa rào ngoài ban công đang to $\rightarrow$ Đúng 0.3 giây trước khi sấm đánh, giảm âm lượng tiếng mưa về 10% (tạo cảm giác nghẹt thở) $\rightarrow$ Sét đánh nổ bùng cực đại!
4. **Cấu hình Timeline Asset (`TL_BalconyScare.playable`):**
   - **Track 1 - Cinemachine Track:**
     - `0.0s -> 1.5s`: Blend êm từ `VCam_PlayerFPS` sang `VCam_BalconyZoom`.
   - **Track 2 - Activation Track (`StrangerInRain`):**
     - Kéo dải Active từ `0.0s` đến đúng `4.0s` (Đúng frame sấm sét chớp lóa thì clip kết thúc $\rightarrow$ Kẻ bí ẩn biến mất tức khắc!).
   - **Track 3 - Animation Track (`VCam_BalconyZoom`):**
     - Keyframe `Lens.FieldOfView` từ `60` co hẹp dần xuống `25` trong khoảng `1.5s -> 4.0s` (Dolly Zoom rợn người bắt trọn gương mặt tối om dưới mũ áo mưa).
   - **Track 4 - Signal Track (`ThunderEffect`):**
     - Tại giây `4.0s`: Đặt `Signal Emitter` gọi `ThunderEffect.TriggerThunder()` $\rightarrow$ Đèn ngoài trời chớp lóa trắng màn hình, camera giật rung bần bật!
     - `4.5s -> 5.5s`: Blend góc nhìn trả về `VCam_PlayerFPS` của Nam.

---

## MODULE 6: NÂNG CẤP GIAO DIỆN ĐIỆN THOẠI ĐA DANH BẠ (`PhoneMenu`)

Cấu trúc giao diện 2 màn hình chuẩn phong cách điện thoại thông minh:

```text
PhonePanel (RectTransform - Nhấn Tab để trượt lên/xuống)
 ├── HeaderBar (Giờ hệ thống, vạch sóng Viettel, % pin)
 ├── Screen_ContactList (Màn hình Danh bạ)
 │    ├── ContactButton_Nem (Button + Chấm đỏ Unread Badge + Preview tin nhắn)
 │    ├── ContactButton_KiEn (Chủ nhà)
 │    └── ContactButton_Police113 (Tổng đài 113)
 └── Screen_ChatDetail (Khung trò chuyện chi tiết)
      ├── TopBar_Chat (Nút [< Quay lại Danh bạ] + Tên người đang nhắn)
      └── ChatScrollRect (Viewport)
           ├── Content_Nem (Chứa bubble tin nhắn của Nem)
           ├── Content_KiEn (Chứa bubble tin nhắn của Chủ nhà)
           └── Content_Police113 (Chứa bubble tin nhắn của 113)
```

**Kỹ thuật Zero-GC Tab Switching:**
- Không `Destroy()` hay xóa danh sách tin nhắn cũ khi đổi người chat.
- Mỗi liên hệ có 1 GameObject `Content_...` riêng. Khi bấm vào danh bạ của ai, chỉ cần bật `Content` của người đó và gán `chatScrollRect.content = Content_...`. Đảm bảo tiến trình gõ chữ (`Typewriter Routine`) chạy ngầm không bao giờ bị giật lag!

---

## MODULE 7: HỆ THỐNG DANH SÁCH VIỆC NHÀ ĐỜI THƯỜNG (`TodoListUI.cs`)

### 7.1. Phân tích Kỹ thuật Checklist từ Video 1 của BatPan (`EcQPcTblr4g`)
Trong Video 1, BatPan xây dựng một hệ thống UI Canvas hiển thị danh sách các nguyên liệu cần chuẩn bị cho món ăn. Trong game kinh dị tâm lý kiểu *Fears to Fathom*, đây chính là **Cơ chế Chữ ký #1 (Signature Mechanic: Mundane To-Do List)**:
1. **Thiết kế Tối giản (Minimalist Diegetic HUD):** Font chữ mỏng màu trắng ở góc trên màn hình, không chiếm dụng tầm nhìn góc nhìn người thứ nhất (FPS).
2. **Kỹ thuật Cập nhật Decoupled (DIP via StoryEventBus):**
   - Script UI KHÔNG tham chiếu trực tiếp hay thăm dò (polling) các GameObject trong Scene.
   - Thay vào đó, nó đăng ký nhận sự kiện `OnStoryFlagChanged` từ [`StoryEventBus.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/Story/StoryEventBus.cs). Khi Nam đặt chìa khóa, bật tivi, đun nước hay ăn mì, `StoryFlag` tương ứng được bật, kích hoạt dòng chữ gạch ngang (`<s>...</s>`) và làm mờ dần sang màu xám.
3. **Hiệu ứng Âm thanh Phản hồi (Audio Feedback):** Một tiếng "tích" giấy note hoặc âm thanh lách cách nhẹ nhàng khi hoàn thành một việc nhà, tạo cảm giác an tâm giả tạo trước khi cơn ác mộng ập đến.

### File: [`Assets/_Project/Scripts/UI/TodoListUI.cs`](file:///e:/Unity/GUT%20FEELING/Assets/_Project/Scripts/UI/TodoListUI.cs)
```csharp
#pragma warning disable CS0414, CS0649
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Giao diện Danh sách Việc nhà Đời thường (Mundane To-Do List).
/// Tái hiện cơ chế Checklist UI trực quan từ Video 1 của BatPan (EcQPcTblr4g)
/// kết hợp phong cách chữ trắng tối giản đặc trưng của dòng game Fears to Fathom.
/// Lắng nghe StoryEventBus để tự động cập nhật, gạch bỏ việc cũ và hiện việc mới.
/// </summary>
public class TodoListUI : MonoBehaviour
{
    [Serializable]
    public class TodoItem
    {
        public string taskDescription;
        public StoryFlag completionFlag;
        [HideInInspector] public bool isCompleted;
    }

    [Header("UI Components")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI todoHeaderLabel;
    [SerializeField] private TextMeshProUGUI todoContentLabel; // Hiển thị các dòng [ ] hoặc [✓]
    [SerializeField] private AudioSource taskCompleteAudio;

    [Header("List Configuration")]
    [SerializeField] private List<TodoItem> act1TodoList = new List<TodoItem>
    {
        new TodoItem { taskDescription = "Kéo khóa mở vali & đặt chìa khóa lên bàn", completionFlag = StoryFlag.KeyPlacedOnTable },
        new TodoItem { taskDescription = "Bật Tivi CRT theo dõi bản tin dự báo bão", completionFlag = StoryFlag.TVNewsWatched },
        new TodoItem { taskDescription = "Hứng nước và đặt ấm lên bếp ga đun sôi", completionFlag = StoryFlag.WaterBoiling },
        new TodoItem { taskDescription = "Chế nước sôi vào bát mì và ăn lót dạ", completionFlag = StoryFlag.NoodleCooked },
        new TodoItem { taskDescription = "Kiểm tra và đóng chốt cửa kính ban công", completionFlag = StoryFlag.SeenStrangerInRain }
    };

    [Header("Fade & Animation Settings")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private bool hideWhenAllCompleted = true;

    private void Start()
    {
        // TODO:
        // 1. Đăng ký lắng nghe sự kiện: StoryEventBus.Instance.OnStoryFlagChanged += HandleStoryFlagChanged
        // 2. Làm mới danh sách hiển thị ban đầu: RefreshTodoListDisplay();
    }

    private void OnDestroy()
    {
        // TODO:
        // Hủy đăng ký StoryEventBus.Instance.OnStoryFlagChanged -= HandleStoryFlagChanged;
    }

    private void HandleStoryFlagChanged(StoryFlag flag)
    {
        // TODO:
        // 1. Duyệt qua act1TodoList tìm TodoItem có completionFlag == flag
        // 2. Nếu tìm thấy và !item.isCompleted:
        //    - Đánh dấu item.isCompleted = true
        //    - Phát âm thanh hoàn thành task (taskCompleteAudio.Play())
        //    - Cập nhật lại chuỗi hiển thị todoContentLabel (hiển thị gạch ngang <s>...</s> hoặc [✓])
        //    - Nếu tất cả đã hoàn thành và hideWhenAllCompleted == true: Chạy Coroutine Fade Alpha về 0
    }

    public void RefreshTodoListDisplay()
    {
        // TODO:
        // Xây dựng chuỗi hiển thị định dạng:
        // - Các task đã hoàn thành: <color=#888888><s>[✓] {taskDescription}</s></color>
        // - Task hiện tại cần làm: <color=#FFFFFF><b>[ ] {taskDescription}</b></color>
        // - Gán chuỗi kết quả vào todoContentLabel.text
    }

    public void SetVisible(bool visible)
    {
        // TODO:
        // Chạy Coroutine Fade Alpha của canvasGroup từ giá trị hiện tại về 1 (nếu visible) hoặc 0 (nếu ẩn)
    }
}
```

---

## MODULE 8: CHECKLIST KẾT NỐI TRONG UNITY EDITOR CHO HỒI 1

1. **Kiểm tra biên dịch C#:** Mở Console Unity, đảm bảo không có bất kỳ lỗi Compiler Error nào.
2. **Cấu hình `Player.cs` & State Machine:**
   - Khai báo: `public PlayerCutsceneState cutsceneState { get; private set; }`
   - Trong `Awake()`: `cutsceneState = new PlayerCutsceneState(this, stateMachine);`
   - Đăng ký: `CutsceneController.Instance.OnCutsceneStarted += () => stateMachine.ChangeState(cutsceneState);`
   - Đăng ký: `CutsceneController.Instance.OnCutsceneEnded += () => stateMachine.ChangeState(freeState);`
3. **Cấu hình `TVController` (BatPan Video 5):**
   - Kéo `VideoPlayer` component vào Inspector.
   - Gán `RenderTexture` (512x384 tỷ lệ 4:3) vào cả `VideoPlayer.targetTexture` và `weatherNewsMat._BaseMap`.
   - Gán `Point Light` vào `screenGlowLight` (đặt Range = 3m, Color = xanh lam nhạt).
   - Đảm bảo video clip dự báo bão được mã hóa định dạng **H.264 MP4** để tránh lỗi giải mã đen màn hình trên Windows URP.
4. **Cấu hình `CoffeeCupItem` & `NoodleBowlController` (BatPan Video 3 & 4):**
   - Gán đúng các Mesh con theo mảng (`biteProgressMeshes[]` cho bát mì và `liquidMeshTransform` cho cốc cà phê).
   - Gán Particle khói (`hotSteamVFX`), kiểm tra cờ tắt khi cạn nước.
   - Gán các Audio Clip húp mì, nhai, húp cà phê, nuốt nước.
5. **Cấu hình `TodoListUI` (BatPan Video 1):**
   - Đặt Canvas ở chế độ Screen Space - Overlay.
   - Neo (Anchor) TextMeshProUGUI ở góc trên bên trái màn hình (`Top-Left`, PosX = 40, PosY = -40).
6. **Cấu hình `ThunderEffect` & Audio Ducking:**
   - Kéo `AudioSource` tiếng mưa ngoài ban công vào trường `rainAudioSource`.
   - Đặt `rainDuckedVolume = 0.15f` để tạo khoảng lặng nghẹt thở 0.3s trước khi tia sét giáng xuống.

---

## MODULE 9: MOCK INTERVIEW CHUẨN BỊ NGHIỆM THU TUẦN 11 - 12

Dựa trên tài liệu [`docs/MOCK_INTERVIEW_CONTEXT_GAME_DEV_TOI.md`](file:///e:/Unity/GUT%20FEELING/docs/MOCK_INTERVIEW_CONTEXT_GAME_DEV_TOI.md), sau khi hoàn thành việc triển khai code Tuần 11 - 12, Senior Mentor sẽ chất vấn bạn 4 câu hỏi thực chiến sau:

1. **Câu 1 (Kiến trúc & OCP):** *"Tại sao trong hệ thống nấu ăn của BatPan, anh ấy dùng Tag chuỗi `CompareTag('Eggs')` trong `Update()` còn trong dự án này chúng ta lại tách thành `HoldableItemBase` và `IInteractable`? Nếu sau này Game Designer muốn thêm 10 món đồ ăn khác thì thiết kế nào ít rủi ro bug hơn?"*
2. **Câu 2 (ISP & Input Routing):** *"Khi người chơi cầm Cốc Cà Phê trên tay và bấm Chuột Phải (`AltUse`), tại sao Cốc Cà Phê bắt buộc phải triển khai cả `IHoldInteractable` lẫn `IAltUsable`? Điều gì sẽ xảy ra nếu ta chỉ triển khai `IAltUsable` trong cấu trúc hiện tại của `PlayerFreeState.cs`?"*
3. **Câu 3 (Performance & GC Alloc):** *"Khi làm Tivi CRT chiếu video và camera giấu kín ở Hồi 2, việc bật Camera phụ `hiddenWallCamera` chạy liên tục từ đầu game sẽ gây ảnh hưởng gì đến Draw Calls và GPU? Chúng ta đã tối ưu vòng đời của nó như thế nào?"*
4. **Câu 4 (Gameplay & Pacing):** *"Tại sao ở Hồi 1 chúng ta bắt buộc Nam chỉ ăn 2 miếng mì tôm rồi dừng lại ở `HalfEaten` và nhấp nửa cốc cà phê mà không cho ăn uống hết sạch? Quyết định kỹ thuật này phục vụ gì cho cú twist kể chuyện kinh dị ở Hồi 2?"*
