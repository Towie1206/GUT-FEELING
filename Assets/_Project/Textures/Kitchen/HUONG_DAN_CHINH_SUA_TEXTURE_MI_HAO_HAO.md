# HƯỚNG DẪN TỰ CHỈNH SỬA TEXTURE LY MÌ HẢO HẢO (HANDY HẢO HẢO)

Tài liệu này hướng dẫn bạn cách tự mở, chỉnh sửa, vẽ đè hoặc thay thế texture của ly mì Handy Hảo Hảo bằng bất kỳ phần mềm đồ họa nào (Photoshop, Paint, Paint.NET, Clip Studio Paint, GIMP) và xem ngay kết quả trên mô hình 3D trong Unity.

---

## 1. VỊ TRÍ FILE TEXTURE CHÍNH
- **Đường dẫn file texture**:
  `Assets/_Project/Textures/Kitchen/T_NoodleCup_HaoHao_Albedo.png`
- **Độ phân giải**: 2048 x 2048 pixels (chuẩn tỷ lệ UV mapping).
- **Material liên kết**: `M_NoodleCup.mat` (Shader: URP Lit).

---

## 2. CÁCH MỞ FILE NHANH NHẤT TRÊN UNITY
Trên thanh menu trên cùng của Unity Editor, bạn chỉ cần bấm:
- **`Tools > Kitchen > Open Noodle Cup Texture (Photoshop or Paint)`**:
  Unity sẽ tự động mở file `T_NoodleCup_HaoHao_Albedo.png` bằng trình chỉnh sửa ảnh mặc định của máy bạn.
- **`Tools > Kitchen > Open Noodle Cup Guide Overlay`**:
  Unity sẽ mở file ảnh hướng dẫn layout UV (`T_NoodleCup_Template_Guide.png`) để bạn đối chiếu các phân vùng.

*(Hoặc trong thẻ Project của Unity, chuột phải vào `T_NoodleCup_HaoHao_Albedo.png` $\rightarrow$ Show in Explorer $\rightarrow$ Mở bằng Photoshop/Paint).*

---

## 3. BẢN ĐỒ PHÂN BỐ CÁC PHÂN VÙNG (UV MAP ATLAS)

File ảnh có kích thước 2048 x 2048 được chia thành các khu vực rõ ràng như sau:

```
+------------------------------------+------------------------------------+
|                                    |                                    |
|          MẶT TRƯỚC LY              |            MẶT SAU LY              |
|        (Chữ Handy Hảo Hảo,         |       (Bảng thành phần dinh dưỡng, |
|          hình tô mì tôm chua cay)   |        mã vạch, logo Acecook)      |
|         X: 0 .. 1024               |        X: 1024 .. 2048             |
|         Y: 1024 .. 2048            |        Y: 1024 .. 2048             |
|                                    |                                    |
+------------------------------------+------------------------------------+
|                                    | [MÌ ĐÃ CHÍN]    | [VẮT MÌ KHÔ]     |
|                                    | X: 1024..1536   | X: 1536..2048    |
|             NẮP LY TRÒN            | Y: 512..1024    | Y: 512..1024     |
|          (Có tai giật mở nắp)      +-----------------+------------------+
|          X: 0 .. 1024              | [NƯỚC SÚP ĐÁY]  | [GIẤY SÁP & NHÔM]|
|          Y: 0 .. 1024              | X: 1024..1536   | X: 1536..2048    |
|                                    | Y: 0..512       | Y: 0..512        |
+------------------------------------+-----------------+------------------+
```

### Chi tiết tọa độ các vùng:
1. **Nửa trên (Y: 1024 đến 2048, X: 0 đến 2048)**:
   - Thân ly hình nón cụt quấn tròn 360 độ.
   - **X: 0 .. 1024** (Bên trái): Mặt trước chính diện của ly mì (Tâm chính diện nằm ở X = 512).
   - **X: 1024 .. 2048** (Bên phải): Mặt sau của ly mì (Bảng nutrition, barcode, hướng dẫn 3 phút).
2. **Góc dưới bên trái (X: 0 đến 1024, Y: 0 đến 1024)**:
   - Nắp đậy hình tròn (Tâm X = 512, Y = 512, Bán kính R = 460px).
   - Tai bóc nắp nằm ở cạnh trên của nắp (X: 450..574, Y: 930..1000).
3. **Góc dưới bên phải - Phần trên (Y: 512 đến 1024)**:
   - **X: 1024 .. 1536**: Mặt trên của vắt mì chín (súp, tôm, ngô ngọt, hành lá).
   - **X: 1536 .. 2048**: Mặt trên của vắt mì khô nguyên bản khi chưa đổ nước sôi.
4. **Góc dưới bên phải - Phần dưới (Y: 0 đến 512)**:
   - **X: 1024 .. 1536**: Nước dùng dưới đáy ly khi ăn hết mì.
   - **X: 1536 .. 1792**: Lớp nhôm tráng bạc mặt dưới của nắp ly.
   - **X: 1792 .. 2048**: Lớp giấy sáp trắng bên trong lòng ly.

---

## 4. QUY TRÌNH CHỈNH SỬA & LƯU
1. Mở file `T_NoodleCup_HaoHao_Albedo.png` trong Photoshop / Paint.
2. Vẽ, chèn chữ, thay đổi màu sắc hoặc paste ảnh bất kỳ vào vùng mong muốn (ví dụ vẽ chữ mới lên mặt trước hoặc đổi màu nắp ly).
3. Nhấn **`Ctrl + S`** (Save) để lưu đè file PNG.
4. Chuyển cửa sổ về **Unity**:
   - Unity tự động nhận diện file thay đổi và nạp lại texture ngay lập tức.
   - Bạn sẽ thấy ly mì trên bàn ăn và trong vali hiển thị đúng hình vẽ mới của bạn.
   - **Lưu ý**: Hệ thống đã được lập trình bảo vệ để **KHÔNG BAO GIỜ tự động ghi đè hay xóa mất hình vẽ của bạn**.

---

## 5. CÁCH KHÔI PHỤC LẠI NGUYÊN BẢN (NẾU VẼ HỎNG)
Nếu bạn lỡ vẽ hỏng hoặc muốn khôi phục lại nhãn mì Handy Hảo Hảo gốc của Acecook:
- Chọn menu: **`Tools > Kitchen > Reset Noodle Cup Texture to Template`**.
- Hệ thống sẽ tự động khôi phục lại texture chuẩn từ kho ảnh mẫu nội bộ của project.
