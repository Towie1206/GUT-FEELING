# BỘ CONTEXT PHỎNG VẤN UNITY DEV (INTERN / JUNIOR / MID) - GAME DEV TỚI

Bộ tài liệu này được tổng hợp từ chuỗi video Mock Interview thực chiến của kênh **Game Dev Tới** (`@gamedevtoi`), làm cơ sở dữ liệu và thang chấm chuẩn cho các buổi **Milestone Mock Interview** trong dự án *GUT FEELING*.

---

## PHẦN 1: LẬP TRÌNH C# CORE, OOP & KIẾN TRÚC PHẦN MỀM (SOFTWARE ARCHITECTURE)

### 1. 4 Tính Chất OOP Cốt Lõi
* **Tính Đóng Gói (Encapsulation)**: Che giấu dữ liệu nhạy cảm bên trong class, chỉ công khai qua Property/Methods với access modifier phù hợp (`private`, `protected`, `public`, `internal`).
* **Tính Kế Thừa (Inheritance)**: Tái sử dụng code từ class cha (`base`), chia sẻ hành vi chung cho các class con.
* **Tính Trừu Tượng (Abstraction)**: Mô hình hóa các khái niệm trừu tượng thông qua `abstract class` và `interface` mà không đi sâu vào chi tiết triển khai cụ thể.
* **Tính Đa Hình (Polymorphism)**:
  * **Compile-time Polymorphism (Overloading)**: Các hàm cùng tên nhưng khác danh sách tham số (khác số lượng hoặc kiểu dữ liệu).
  * **Runtime Polymorphism (Overriding)**: Class con ghi đè triển khai của class cha thông qua `virtual` / `override` hoặc `interface`.

### 2. Cấu Trúc Dữ Liệu: Array vs. List vs. LinkedList
* **Array (Mảng tĩnh)**: 
  * Cố định kích thước ngay từ khi khởi tạo, cấp phát trên các ô nhớ liên tục (Contiguous Memory).
  * Tốc độ truy xuất phần tử theo index đạt $O(1)$, cache locality cực tốt.
* **List<T> (Danh sách động)**:
  * Được xây dựng trên nền tảng Array ẩn bên dưới (Underlying Array).
  * Khi số phần tử vượt quá dung lượng hiện tại (`Capacity`), List tự động tạo một Array mới có kích thước gấp đôi (thường $\times 2$), sao chép dữ liệu cũ sang và giải phóng Array cũ $\rightarrow$ Có thể sinh ra Garbage Collection (GC Alloc) nếu mảng mở rộng liên tục.
* **LinkedList<T> (Danh sách liên kết)**:
  * Mỗi phần tử là một Node chứa dữ liệu và con trỏ trỏ tới Node tiếp theo/trước đó.
  * Các ô nhớ nằm rải rác trong Heap, không liên tục $\rightarrow$ Kém cache locality nhưng chèn/xóa phần tử ở giữa nhanh $O(1)$ nếu đã có node.

### 3. Design Patterns Phổ Biến Trong Game Dev
* **Singleton Pattern**:
  * *Ưu điểm*: Cung cấp một điểm truy cập toàn cục duy nhất (`Instance`), tiện lợi cho các Manager (`AudioManager`, `GameManager`).
  * *Nhược điểm*: Gây phụ thuộc chặt chẽ (Tight Coupling), khó Unit Test, khó quản lý thứ tự khởi tạo giữa các Scene.
* **Observer Pattern (Publish - Subscribe)**:
  * Hệ thống phát và lắng nghe sự kiện (`event`, `Action<T>`, `Func<T>`).
  * Giúp tách rời (Decoupling) giữa đối tượng phát tín hiệu và đối tượng nhận tín hiệu.
  * *Quy tắc vàng*: Luôn luôn hủy đăng ký sự kiện (`-=`) trong `OnDisable()` hoặc `OnDestroy()` để tránh Memory Leak!
* **State Pattern (FSM - Finite State Machine)**:
  * Mỗi trạng thái là một Class riêng kế thừa `StateBase`.
  * Đảm bảo tính loại trừ lẫn nhau (Mutually Exclusive) – tại 1 thời điểm chỉ ở duy nhất 1 state.
  * Phân biệt với **Behavior Tree (BT)**: BT phù hợp cho AI phức tạp nhờ các Composite Nodes (`Selector`, `Sequence`), hỗ trợ song song và phân cấp.
* **Strategy Pattern & DIP (Dependency Inversion)**:
  * Dùng `Interface` để tráo đổi linh hoạt các thuật toán/hành vi (ví dụ: các chiến thuật di chuyển khác nhau) ở runtime mà không cần sửa code cũ.
* **Object Pooling Pattern**:
  * Khởi tạo sẵn một danh sách GameObject (`Queue<GameObject>` hoặc `List<GameObject>`) khi bắt đầu game.
  * Khi cần dùng: `SetActive(true)` lấy từ Pool ra.
  * Khi dùng xong: `SetActive(false)` trả về lại Pool.
  * Tuyệt đối tránh gọi `Instantiate()` và `Destroy()` liên tục trong gameplay (như bắn đạn, nhặt coin, spawn máu) vì sẽ gây phân mảnh bộ nhớ và kích hoạt GC Spike gây giật khung hình.

### 4. Clean Code & Kỹ Năng Debug Chuyên Nghiệp
* **DRY (Don't Repeat Yourself)**: Không lặp lại logic; nếu logic xuất hiện 3 lần, hãy gom thành hàm dùng chung.
* **Tránh Magic Numbers & Magic Strings**: Định nghĩa hằng số (`const`) hoặc `enum` thay vì hardcode giá trị `3.14f`, `"Player"`, layer số `7`.
* **Kỹ năng Debug**: 
  * Không chỉ dùng `Debug.Log` (vì `Debug.Log` sinh ra chuỗi string gây tốn bộ nhớ và rác log).
  * Bắt buộc biết cắm **BreakPoint** trong IDE (Visual Studio / Rider), bước qua từng dòng code (`Step Over`, `Step Into`), đọc giá trị biến runtime và sử dụng chế độ **Debug Inspector** của Unity.

---

## PHẦN 2: VÒNG ĐỜI & HỆ THỐNG CỐT LÕI UNITY ENGINE

### 1. Vòng Đời Script (Unity Execution Order)
* **`Awake()`**: Gọi ngay khi GameObject/Script được load, chỉ gọi 1 lần. Chuyên dùng để khởi tạo biến nội bộ và `GetComponent`.
* **`OnEnable()`**: Gọi mỗi khi script được kích hoạt. Nơi đăng ký các Event (`+=`).
* **`Start()`**: Gọi trước frame đầu tiên sau khi script active. Dùng khi cần giao tiếp dữ liệu với các GameObject khác đã `Awake`.
* **`FixedUpdate()`**: Gọi theo chu kỳ thời gian cố định (`fixedDeltaTime = 0.02s` mặc định = 50 lần/giây), độc lập hoàn toàn với FPS. Dành riêng cho tính toán vật lý (`Rigidbody`, `AddForce`).
* **`Update()`**: Gọi mỗi frame hiển thị, phụ thuộc vào tốc độ khung hình (FPS) của máy.
* **`LateUpdate()`**: Gọi sau khi toàn bộ `Update` của tất cả script đã chạy xong. Dành cho Camera Follow để tránh giật lag camera theo sau nhân vật.
* **`OnDisable()`**: Gọi khi script bị tắt. Nơi bắt buộc hủy đăng ký Event (`-=`).
* **`OnDestroy()`**: Gọi khi GameObject bị xóa vĩnh viễn.

### 2. Ý Nghĩa Của `Time.deltaTime`
* Đại diện cho khoảng thời gian trôi qua giữa frame trước và frame hiện tại.
* Giúp triệt tiêu độ chênh lệch FPS:
  * Máy 30 FPS: mỗi frame chạy mất $1/30\text{s}$ ($\approx 0.033\text{s}$).
  * Máy 120 FPS: mỗi frame chạy mất $1/120\text{s}$ ($\approx 0.008\text{s}$).
* Nhân `Time.deltaTime` vào vận tốc giúp nhân vật trên cả 2 máy đều di chuyển được đúng quãng đường như nhau trong 1 giây thực tế.

### 3. Hệ Thống Vật Lý (Physics & Collision)
* **Điều kiện bắt va chạm (`OnCollisionEnter` / `OnTriggerEnter`)**:
  * Cả hai vật thể đều phải có `Collider`.
  * **BẮT BUỘC ít nhất 1 trong 2 vật thể phải có `Rigidbody`** (vật thể di chuyển nên mang Rigidbody).
* **Collider vs Trigger**:
  * `Collider` thường (`isTrigger = false`): Tạo lực đẩy va chạm cứng, không cho xuyên qua.
  * `Trigger` (`isTrigger = true`): Cho phép xuyên qua hoàn toàn, chỉ bắn sự kiện `OnTriggerEnter` (dành cho vùng nhặt đồ, bẫy tàng hình, vùng kích hoạt cutscene).
* **Kinematic Rigidbody (`isKinematic = true`)**:
  * Không chịu tác động của trọng lực hay lực đẩy va chạm từ môi trường.
  * Vị trí hoàn toàn do script hoặc animation điều khiển, nhưng vẫn có khả năng kích hoạt trigger va chạm vào các vật thể khác.

### 4. Quản Lý Scene & Tối Ưu Loading Screen
* **`LoadSceneMode.Single` vs `LoadSceneMode.Additive`**:
  * `Single`: Dọn dẹp xóa sổ toàn bộ Scene cũ để nạp Scene mới.
  * `Additive`: Giữ nguyên Scene hiện tại và nạp thêm Scene mới vào chạy song song (rất thích hợp để tách riêng Scene UI HUD, Scene Audio, Scene Gameplay).
* **Đồng bộ vs Bất đồng bộ**:
  * `SceneManager.LoadScene()`: Chặn cứng Main Thread cho đến khi load xong $\rightarrow$ Treo game, đứng hình.
  * `SceneManager.LoadSceneAsync()`: Nạp ngầm dưới background, cho phép đọc `asyncOperation.progress` để cập nhật thanh Loading bar mượt mà.
* **Hạn chế thư mục `Resources`**: Unity bắt buộc phải quét và nạp toàn bộ danh mục metadata của thư mục `Resources` ngay khi khởi động game, làm kéo dài thời gian mở game trên máy yếu. Thay vào đó nên dùng **Addressables**.

### 5. ScriptableObject
* Lưu trữ dữ liệu cấu hình tĩnh (Config Data: chỉ số quái, thông số súng, item database) tồn tại độc lập với Scene.
* Tiết kiệm RAM vì hàng trăm instance enemy trong game có thể cùng trỏ về 1 file ScriptableObject duy nhất thay vì nhân bản dữ liệu biến.
* *Lưu ý*: Không dùng ScriptableObject để lưu dữ liệu Save Game của người chơi ở bản Build thực tế vì thay đổi ở runtime sẽ không lưu lại trên đĩa sau khi tắt app.

---

## PHẦN 3: TỐI ƯU HÓA HIỆU NĂNG (GAME OPTIMIZATION)

### 1. Tối Ưu Dung Lượng Build (Build Size)
* **Nén Texture**: Dùng định dạng nén chuẩn cho từng nền tảng (ví dụ **ASTC** cho Mobile/WebGL hiện đại, **ETC2** cho Android cũ). Giảm Max Size của texture nếu không cần thiết.
* **Sprite Atlas**: Đóng gói nhiều sprite 2D nhỏ vào 1 tấm ảnh lớn để gom Batch và giảm Draw Calls.
* **Build Report**: Mở Editor Log sau khi build để xem danh sách tỉ lệ phần trăm dung lượng chiếm dụng (Textures, Meshes, Audio, Shaders).
* **Addressables Asset System**: Cho phép tách nhỏ các asset nặng đưa lên Cloud/Server để người chơi tải ngầm sau khi cài game.

### 2. Tối Ưu Hiệu Năng Runtime (CPU / GPU / Memory)
* **3 Chỉ số quan trọng trong Profiler / Stats**:
  * **Draw Calls / Batches**: Số lần CPU ra lệnh cho GPU vẽ hình.
  * **Triangles (Tris) & Vertices (Verts)**: Số lượng đa giác hiển thị trên màn hình.
  * **GC Alloc**: Lượng rác bộ nhớ phát sinh trong frame gây treo máy khi Garbage Collector dọn dẹp.
* **Kỹ thuật Raycast vs Projectile**:
  * Bắn đạn súng trường, súng lục: Dùng **Raycast** tính toán trúng đích tức thì, kết hợp hiệu ứng tia lửa giả $\rightarrow$ Cực nhẹ, không tốn tính toán va chạm vật lý.
* **Tối ưu Component Lookup**:
  * Dùng `TryGetComponent<T>()` hoặc Interface thay vì dùng chuỗi string `CompareTag("...")` hay `GetComponent(string)`.
* **Transform vs Rigidbody**: Di chuyển vật thể bằng `transform.position` nhẹ hơn rất nhiều so với `Rigidbody` vì không kích hoạt thuật toán tính lực vật lý của PhysX engine.

---

## PHẦN 4: HỆ THỐNG LƯU TRỮ DỮ LIỆU & BẢO MẬT (SAVE SYSTEM)
* **`PlayerPrefs`**: Chỉ thích hợp lưu các cài đặt cơ bản như Volume, Độ phân giải, Độ nhạy chuột. Tuyệt đối không dùng lưu dữ liệu game phức tạp hay tiền tệ vì file lưu dưới dạng plain text trong Registry (Windows) rất dễ bị người chơi sửa gian lận.
* **Lưu File Local (`JSON` / `Binary`)**:
  * Serialize dữ liệu game thành chuỗi JSON (`JsonUtility.ToJson`) hoặc Binary.
  * Ghi vào thư mục `Application.persistentDataPath` (đường dẫn an toàn không bị xóa khi update game).
* **Mã Hóa Dữ Liệu (Encryption)**: Dùng thuật toán mã hóa (như AES đối xứng hoặc XOR đơn giản) để mã hóa chuỗi JSON trước khi ghi xuống đĩa, tránh bị người chơi mở Notepad chỉnh sửa chỉ số.

---

## PHẦN 5: KỸ NĂNG NGHỀ NGHIỆP, TƯ DUY & THÁI ĐỘ LÀM GAME
* **Xây dựng Portfolio & CV**: Phải có link GitHub code sạch, video gameplay demo 60s và bản Build chơi thử được. Dự án nên có cả 2D và 3D.
* **Không học vẹt từ khóa**: Trả lời phỏng vấn cần nêu được bản chất, ưu và nhược điểm thực tế chứ không chỉ học thuộc lòng định nghĩa trên mạng.
* **Thái độ sử dụng AI (ChatGPT / Antigravity)**: Sử dụng AI để học hỏi tư duy, sinh form code chuẩn, nhưng **bắt buộc phải hiểu cặn kẽ từng dòng code và tự tay sửa lỗi được**.
