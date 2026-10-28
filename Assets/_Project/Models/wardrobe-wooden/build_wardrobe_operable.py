import bpy
import bmesh
import mathutils
import math

bpy.ops.wm.read_factory_settings(use_empty=True)

fbx_source = r"E:\Unity\GUT FEELING\Assets\_Project\Models\wardrobe-wooden\source\Wardrobe.fbx"
bpy.ops.import_scene.fbx(filepath=fbx_source)

orig_obj = bpy.data.objects['Wardrobe.001']
orig_mesh = orig_obj.data

# World scale factors to bake:
# Original mesh dimensions in Blender:
# dx = 1.0009 -> scaled by 2.25 to 2.2519m
# dy = 0.6011 -> scaled by 1.50 to 0.9016m
# dz = 1.6073 -> scaled by 1.50 to 2.4109m
sx = 2.25
sy = 1.50
sz = 1.50

mat_wardrobe = bpy.data.materials.get('Wardrobe')
mat_handle = bpy.data.materials.get('WardrobeHandle')

bm_orig = bmesh.new()
bm_orig.from_mesh(orig_mesh)
bm_orig.faces.ensure_lookup_table()
bm_orig.verts.ensure_lookup_table()
uv_orig = bm_orig.loops.layers.uv.verify()

# Key coordinates in scaled space (logical wardrobe space):
# Left is -X, Right is +X, Back is -Y, Front is +Y, Bottom is -Z, Top is +Z
dx_l_out = -0.4758 * sx  # -1.07055m
dx_l_in  = -0.0015       # -0.00150m (3mm realistic center meeting gap)
dx_r_in  = +0.0015       # +0.00150m
dx_r_out = +0.4758 * sx  # +1.07055m

dz_bot = -0.7844 * sz   # -1.1766m
dz_top = +0.7844 * sz   # +1.1766m
cavity_z_bot = -0.7200 * sz  # -1.0800m (Raised ~9.6cm inside to form bottom plinth/shelf above room floor)

dy_front = 0.3009 * sy  # +0.45135m (door front face)
dy_back  = 0.2780 * sy  # +0.41700m (door thickness = 0.03435m ~ 3.4cm)
dy_bevel = 0.3236 * sy  # +0.48540m (frame front face / bevel)

cavity_y_back = -0.2400 * sy  # -0.36000m (interior backboard, 3cm wall thickness)
shelf_z_bot   = 0.2000 * sz   # +0.3000m
shelf_z_top   = 0.2250 * sz   # +0.3375m
shelf_y_front = 0.2700 * sy   # +0.4050m

# -------------------------------------------------------------------------
# Helper functions
# Note: To orient properly for Unity:
# We want in Unity:
#   X_unity = X_blender
#   Y_unity = Z_blender (Up)
#   Z_unity = Y_blender (Forward)
# In Blender export to Unity with FBX:
# Standard Blender FBX export transforms: X_unity = -X_b, Y_unity = Z_b, Z_unity = -Y_b.
# So by mapping in Blender:
#   x_b = -x_target
#   y_b = -y_target
#   z_b = z_target
# Blender FBX export produces EXACTLY (x_target, z_target, y_target) in Unity!
# -------------------------------------------------------------------------

def map_co(x, y, z):
    return mathutils.Vector((-x, -y, z))

def add_quad_direct(bm, uv_layer, p1, p2, p3, p4, uv1, uv2, uv3, uv4, mat_idx=0):
    v1 = bm.verts.new(map_co(*p1))
    v2 = bm.verts.new(map_co(*p2))
    v3 = bm.verts.new(map_co(*p3))
    v4 = bm.verts.new(map_co(*p4))
    f = bm.faces.new([v1, v2, v3, v4])
    f.material_index = mat_idx
    f.loops[0][uv_layer].uv = mathutils.Vector(uv1)
    f.loops[1][uv_layer].uv = mathutils.Vector(uv2)
    f.loops[2][uv_layer].uv = mathutils.Vector(uv3)
    f.loops[3][uv_layer].uv = mathutils.Vector(uv4)
    return f

# ==========================================
# 1. BUILD WARDROBE FRAME
# ==========================================
bm_frame = bmesh.new()
uv_frame = bm_frame.loops.layers.uv.new("UVMap")

# Door faces and front gap faces from original mesh to exclude from frame:
door_and_gap_faces = {42, 22, 25, 23, 43, 52, 6, 27, 24, 53, 50, 28, 44, 26, 51}

# Copy existing exterior faces scaled and mapped
for f in bm_orig.faces:
    if f.index >= 54:
        continue
    if f.index in door_and_gap_faces:
        continue
    new_verts = []
    for loop in f.loops:
        v = loop.vert
        co_scaled = (v.co.x * sx, v.co.y * sy, v.co.z * sz)
        new_verts.append(bm_frame.verts.new(map_co(*co_scaled)))
    try:
        nf = bm_frame.faces.new(new_verts)
        nf.material_index = 0
        for i, loop in enumerate(nf.loops):
            loop[uv_frame].uv = f.loops[i][uv_orig].uv
    except ValueError:
        pass

# Add Interior Cavity faces with precise inward-pointing normals:

# Left inside wall (facing -X in Blender / +X in Unity inward into cavity):
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_front, cavity_z_bot),
    (dx_l_out, dy_front, dz_top),
    (dx_l_out, cavity_y_back, dz_top),
    (dx_l_out, cavity_y_back, cavity_z_bot),
    (0.575, 0.455),
    (0.575, 0.909),
    (0.724, 0.909),
    (0.724, 0.455),
    mat_idx=0)

# Right inside wall (facing +X in Blender / -X in Unity inward into cavity):
add_quad_direct(bm_frame, uv_frame,
    (dx_r_out, cavity_y_back, cavity_z_bot),
    (dx_r_out, cavity_y_back, dz_top),
    (dx_r_out, dy_front, dz_top),
    (dx_r_out, dy_front, cavity_z_bot),
    (0.724, 0.455),
    (0.724, 0.909),
    (0.874, 0.909),
    (0.874, 0.455),
    mat_idx=0)

# Backboard inside (facing +Y forward towards room):
# Winding: top-left -> top-right -> bot-right -> bot-left
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, cavity_y_back, dz_top),
    (dx_r_out, cavity_y_back, dz_top),
    (dx_r_out, cavity_y_back, cavity_z_bot),
    (dx_l_out, cavity_y_back, cavity_z_bot),
    (0.580, 0.420),
    (0.840, 0.420),
    (0.840, 0.050),
    (0.580, 0.050),
    mat_idx=0)

# Cavity floor (facing +Z upward into cavity):
# Winding: bot-left -> bot-right -> front-right -> front-left
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, cavity_y_back, cavity_z_bot),
    (dx_r_out, cavity_y_back, cavity_z_bot),
    (dx_r_out, dy_front, cavity_z_bot),
    (dx_l_out, dy_front, cavity_z_bot),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.250),
    (0.580, 0.250),
    mat_idx=0)

# Front plinth kickboard face (facing +Y forward towards room):
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_front, cavity_z_bot),
    (dx_r_out, dy_front, cavity_z_bot),
    (dx_r_out, dy_front, dz_bot),
    (dx_l_out, dy_front, dz_bot),
    (0.580, 0.120),
    (0.840, 0.120),
    (0.840, 0.050),
    (0.580, 0.050),
    mat_idx=0)

# Cavity ceiling (facing -Z downward into cavity):
# Winding: front-left -> front-right -> back-right -> back-left
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_front, dz_top),
    (dx_r_out, dy_front, dz_top),
    (dx_r_out, cavity_y_back, dz_top),
    (dx_l_out, cavity_y_back, dz_top),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.250),
    (0.580, 0.250),
    mat_idx=0)

# Horizontal Shelf
# Top face (facing +Z upward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, cavity_y_back, shelf_z_top),
    (dx_r_out, cavity_y_back, shelf_z_top),
    (dx_r_out, shelf_y_front, shelf_z_top),
    (dx_l_out, shelf_y_front, shelf_z_top),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.250),
    (0.580, 0.250),
    mat_idx=0)

# Bottom face (facing -Z downward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, shelf_y_front, shelf_z_bot),
    (dx_r_out, shelf_y_front, shelf_z_bot),
    (dx_r_out, cavity_y_back, shelf_z_bot),
    (dx_l_out, cavity_y_back, shelf_z_bot),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.250),
    (0.580, 0.250),
    mat_idx=0)

# Front edge (facing +Y forward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, shelf_y_front, shelf_z_top),
    (dx_r_out, shelf_y_front, shelf_z_top),
    (dx_r_out, shelf_y_front, shelf_z_bot),
    (dx_l_out, shelf_y_front, shelf_z_bot),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.020),
    (0.580, 0.020),
    mat_idx=0)

# Frame Opening Rim Lips (connecting inner cavity front edge to outer frame bevel at dy_bevel)
# Top lip (facing -Z downward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_bevel, dz_top),
    (dx_r_out, dy_bevel, dz_top),
    (dx_r_out, dy_front, dz_top),
    (dx_l_out, dy_front, dz_top),
    (0.580, 0.050),
    (0.840, 0.050),
    (0.840, 0.020),
    (0.580, 0.020),
    mat_idx=0)

# Bottom lip (facing +Z upward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_bevel, dz_bot),
    (dx_r_out, dy_bevel, dz_bot),
    (dx_r_out, dy_front, dz_bot),
    (dx_l_out, dy_front, dz_bot),
    (0.580, 0.020),
    (0.840, 0.020),
    (0.840, 0.050),
    (0.580, 0.050),
    mat_idx=0)

# Left lip (facing -X in Blender / +X in Unity inward)
add_quad_direct(bm_frame, uv_frame,
    (dx_l_out, dy_bevel, dz_bot),
    (dx_l_out, dy_bevel, dz_top),
    (dx_l_out, dy_front, dz_top),
    (dx_l_out, dy_front, dz_bot),
    (0.800, 0.480),
    (0.800, 0.900),
    (0.840, 0.900),
    (0.840, 0.480),
    mat_idx=0)

# Right lip (facing +X in Blender / -X in Unity inward)
add_quad_direct(bm_frame, uv_frame,
    (dx_r_out, dy_front, dz_bot),
    (dx_r_out, dy_front, dz_top),
    (dx_r_out, dy_bevel, dz_top),
    (dx_r_out, dy_bevel, dz_bot),
    (0.800, 0.480),
    (0.800, 0.900),
    (0.840, 0.900),
    (0.840, 0.480),
    mat_idx=0)

# Clothes Hanging Rod below shelf:
rod_y = 0.050 * sy
rod_z = 0.150 * sz
rod_r = 0.015 * sz
rod_segs = 8
rod_pts_l = []
rod_pts_r = []
for s in range(rod_segs):
    ang = 2.0 * math.pi * s / rod_segs
    dy = rod_r * math.cos(ang)
    dz = rod_r * math.sin(ang)
    rod_pts_l.append((dx_l_out, rod_y + dy, rod_z + dz))
    rod_pts_r.append((dx_r_out, rod_y + dy, rod_z + dz))
for s in range(rod_segs):
    s_next = (s + 1) % rod_segs
    add_quad_direct(bm_frame, uv_frame,
        rod_pts_l[s], rod_pts_r[s], rod_pts_r[s_next], rod_pts_l[s_next],
        (0.375, s/rod_segs),
        (0.625, s/rod_segs),
        (0.625, (s+1)/rod_segs),
        (0.375, (s+1)/rod_segs),
        mat_idx=1)

mesh_frame = bpy.data.meshes.new('Wardrobe_Frame')
bm_frame.to_mesh(mesh_frame)
bm_frame.free()

obj_frame = bpy.data.objects.new('Wardrobe_Frame', mesh_frame)
obj_frame.data.materials.append(mat_wardrobe)
obj_frame.data.materials.append(mat_handle)
bpy.context.collection.objects.link(obj_frame)


# ==========================================
# 2. BUILD WARDROBE DOOR LEFT
# ==========================================
hinge_l_logical = (dx_l_out, dy_front, 0.0)
hinge_l_blender = map_co(*hinge_l_logical)

bm_dl = bmesh.new()
uv_dl = bm_dl.loops.layers.uv.new("UVMap")

def add_door_quad_l_uv(p1, p2, p3, p4, uv1, uv2, uv3, uv4, mat_idx=0):
    def to_local(p):
        v_world = map_co(*p)
        return v_world - hinge_l_blender
    v1 = bm_dl.verts.new(to_local(p1))
    v2 = bm_dl.verts.new(to_local(p2))
    v3 = bm_dl.verts.new(to_local(p3))
    v4 = bm_dl.verts.new(to_local(p4))
    f = bm_dl.faces.new([v1, v2, v3, v4])
    f.material_index = mat_idx
    f.loops[0][uv_dl].uv = mathutils.Vector(uv1)
    f.loops[1][uv_dl].uv = mathutils.Vector(uv2)
    f.loops[2][uv_dl].uv = mathutils.Vector(uv3)
    f.loops[3][uv_dl].uv = mathutils.Vector(uv4)
    return f

# Left door front face (+Y normal facing room):
# Original FBX Face 42 UVs:
# Outer-Bottom (-0.4758, -0.7844) -> (0.1374, 0.9119)
# Inner-Bottom (-0.0110, -0.7844) -> (0.2708, 0.9119)
# Inner-Top    (-0.0110, +0.7844) -> (0.2708, 0.4615)
# Outer-Top    (-0.4758, +0.7844) -> (0.1374, 0.4615)
add_door_quad_l_uv(
    (dx_l_out, dy_front, dz_bot),
    (dx_l_in,  dy_front, dz_bot),
    (dx_l_in,  dy_front, dz_top),
    (dx_l_out, dy_front, dz_top),
    (0.1374, 0.9119),
    (0.2708, 0.9119),
    (0.2708, 0.4615),
    (0.1374, 0.4615),
    mat_idx=0
)

# Left door back face (-Y normal facing cavity when closed, room when open):
add_door_quad_l_uv(
    (dx_l_out, dy_back, dz_bot),
    (dx_l_out, dy_back, dz_top),
    (dx_l_in,  dy_back, dz_top),
    (dx_l_in,  dy_back, dz_bot),
    (0.1374, 0.9119),
    (0.1374, 0.4615),
    (0.2708, 0.4615),
    (0.2708, 0.9119),
    mat_idx=0
)

# Outer edge (-X normal, hinge edge):
add_door_quad_l_uv(
    (dx_l_out, dy_back, dz_bot),
    (dx_l_out, dy_front, dz_bot),
    (dx_l_out, dy_front, dz_top),
    (dx_l_out, dy_back, dz_top),
    (0.486, 0.912),
    (0.492, 0.912),
    (0.492, 0.461),
    (0.486, 0.461),
    mat_idx=0
)

# Inner edge (+X normal, center meeting edge):
add_door_quad_l_uv(
    (dx_l_in, dy_front, dz_bot),
    (dx_l_in, dy_back, dz_bot),
    (dx_l_in, dy_back, dz_top),
    (dx_l_in, dy_front, dz_top),
    (0.492, 0.912),
    (0.497, 0.912),
    (0.497, 0.461),
    (0.492, 0.461),
    mat_idx=0
)

# Top edge (+Z normal):
add_door_quad_l_uv(
    (dx_l_out, dy_front, dz_top),
    (dx_l_in,  dy_front, dz_top),
    (dx_l_in,  dy_back,  dz_top),
    (dx_l_out, dy_back,  dz_top),
    (0.468, 0.601),
    (0.474, 0.601),
    (0.474, 0.735),
    (0.468, 0.735),
    mat_idx=0
)

# Bottom edge (-Z normal):
add_door_quad_l_uv(
    (dx_l_out, dy_front, dz_bot),
    (dx_l_out, dy_back,  dz_bot),
    (dx_l_in,  dy_back,  dz_bot),
    (dx_l_in,  dy_front, dz_bot),
    (0.457, 0.461),
    (0.457, 0.595),
    (0.463, 0.595),
    (0.463, 0.461),
    mat_idx=0
)

# Left handle (Island 1: faces 54 to 74)
handle_l_verts = {}
for f in bm_orig.faces:
    if 54 <= f.index <= 74:
        new_verts = []
        for loop in f.loops:
            v = loop.vert
            if v not in handle_l_verts:
                p_log = (v.co.x * sx, v.co.y * sy, v.co.z * sz)
                v_blender = map_co(*p_log) - hinge_l_blender
                handle_l_verts[v] = bm_dl.verts.new(v_blender)
            new_verts.append(handle_l_verts[v])
        try:
            nf = bm_dl.faces.new(new_verts)
            nf.material_index = 1
            for i, loop in enumerate(nf.loops):
                loop[uv_dl].uv = f.loops[i][uv_orig].uv
        except ValueError:
            pass

mesh_dl = bpy.data.meshes.new('Wardrobe_Door_Left')
bm_dl.to_mesh(mesh_dl)
bm_dl.free()

obj_dl = bpy.data.objects.new('Wardrobe_Door_Left', mesh_dl)
obj_dl.location = hinge_l_blender
obj_dl.data.materials.append(mat_wardrobe)
obj_dl.data.materials.append(mat_handle)
bpy.context.collection.objects.link(obj_dl)


# ==========================================
# 3. BUILD WARDROBE DOOR RIGHT
# ==========================================
hinge_r_logical = (dx_r_out, dy_front, 0.0)
hinge_r_blender = map_co(*hinge_r_logical)

bm_dr = bmesh.new()
uv_dr = bm_dr.loops.layers.uv.new("UVMap")

def add_door_quad_r_uv(p1, p2, p3, p4, uv1, uv2, uv3, uv4, mat_idx=0):
    def to_local(p):
        v_world = map_co(*p)
        return v_world - hinge_r_blender
    v1 = bm_dr.verts.new(to_local(p1))
    v2 = bm_dr.verts.new(to_local(p2))
    v3 = bm_dr.verts.new(to_local(p3))
    v4 = bm_dr.verts.new(to_local(p4))
    f = bm_dr.faces.new([v1, v2, v3, v4])
    f.material_index = mat_idx
    f.loops[0][uv_dr].uv = mathutils.Vector(uv1)
    f.loops[1][uv_dr].uv = mathutils.Vector(uv2)
    f.loops[2][uv_dr].uv = mathutils.Vector(uv3)
    f.loops[3][uv_dr].uv = mathutils.Vector(uv4)
    return f

# Right door front face (+Y normal facing room):
# Original FBX Face 22 UVs:
# Inner-Bottom (-0.0027, -0.7844) -> (0.0000, 0.9119)
# Outer-Bottom (+0.4758, -0.7844) -> (0.1374, 0.9119)
# Outer-Top    (+0.4758, +0.7844) -> (0.1374, 0.4615)
# Inner-Top    (-0.0027, +0.7844) -> (0.0000, 0.4615)
add_door_quad_r_uv(
    (dx_r_in,  dy_front, dz_bot),
    (dx_r_out, dy_front, dz_bot),
    (dx_r_out, dy_front, dz_top),
    (dx_r_in,  dy_front, dz_top),
    (0.0000, 0.9119),
    (0.1374, 0.9119),
    (0.1374, 0.4615),
    (0.0000, 0.4615),
    mat_idx=0
)

# Right door back face (-Y normal facing cavity when closed, room when open):
add_door_quad_r_uv(
    (dx_r_in,  dy_back, dz_bot),
    (dx_r_in,  dy_back, dz_top),
    (dx_r_out, dy_back, dz_top),
    (dx_r_out, dy_back, dz_bot),
    (0.0000, 0.9119),
    (0.0000, 0.4615),
    (0.1374, 0.4615),
    (0.1374, 0.9119),
    mat_idx=0
)

# Outer edge (+X normal, hinge edge):
add_door_quad_r_uv(
    (dx_r_out, dy_front, dz_bot),
    (dx_r_out, dy_back,  dz_bot),
    (dx_r_out, dy_back,  dz_top),
    (dx_r_out, dy_front, dz_top),
    (0.479, 0.912),
    (0.486, 0.912),
    (0.486, 0.461),
    (0.479, 0.461),
    mat_idx=0
)

# Inner edge (-X normal, center meeting edge):
add_door_quad_r_uv(
    (dx_r_in, dy_back,  dz_bot),
    (dx_r_in, dy_front, dz_bot),
    (dx_r_in, dy_front, dz_top),
    (dx_r_in, dy_back,  dz_top),
    (0.497, 0.912),
    (0.501, 0.912),
    (0.501, 0.461),
    (0.497, 0.461),
    mat_idx=0
)

# Top edge (+Z normal):
add_door_quad_r_uv(
    (dx_r_in,  dy_front, dz_top),
    (dx_r_out, dy_front, dz_top),
    (dx_r_out, dy_back,  dz_top),
    (dx_r_in,  dy_back,  dz_top),
    (0.468, 0.461),
    (0.474, 0.461),
    (0.474, 0.599),
    (0.468, 0.599),
    mat_idx=0
)

# Bottom edge (-Z normal):
add_door_quad_r_uv(
    (dx_r_in,  dy_front, dz_bot),
    (dx_r_in,  dy_back,  dz_bot),
    (dx_r_out, dy_back,  dz_bot),
    (dx_r_out, dy_front, dz_bot),
    (0.457, 0.457),
    (0.457, 0.735),
    (0.463, 0.735),
    (0.463, 0.457),
    mat_idx=0
)

# Right handle (Island 2: faces 75 to 95)
handle_r_verts = {}
for f in bm_orig.faces:
    if 75 <= f.index <= 95:
        new_verts = []
        for loop in f.loops:
            v = loop.vert
            if v not in handle_r_verts:
                p_log = (v.co.x * sx, v.co.y * sy, v.co.z * sz)
                v_blender = map_co(*p_log) - hinge_r_blender
                handle_r_verts[v] = bm_dr.verts.new(v_blender)
            new_verts.append(handle_r_verts[v])
        try:
            nf = bm_dr.faces.new(new_verts)
            nf.material_index = 1
            for i, loop in enumerate(nf.loops):
                loop[uv_dr].uv = f.loops[i][uv_orig].uv
        except ValueError:
            pass

mesh_dr = bpy.data.meshes.new('Wardrobe_Door_Right')
bm_dr.to_mesh(mesh_dr)
bm_dr.free()

obj_dr = bpy.data.objects.new('Wardrobe_Door_Right', mesh_dr)
obj_dr.location = hinge_r_blender
obj_dr.data.materials.append(mat_wardrobe)
obj_dr.data.materials.append(mat_handle)
bpy.context.collection.objects.link(obj_dr)

# Clean up original
bpy.data.objects.remove(orig_obj)
bm_orig.free()

# Create Root Empty
root_obj = bpy.data.objects.new('Wardrobe_Wooden', None)
root_obj.location = (0, 0, 0)
bpy.context.collection.objects.link(root_obj)

obj_frame.parent = root_obj
obj_dl.parent = root_obj
obj_dr.parent = root_obj

# Select all
for o in bpy.context.selected_objects:
    o.select_set(False)
root_obj.select_set(True)
obj_frame.select_set(True)
obj_dl.select_set(True)
obj_dr.select_set(True)
bpy.context.view_layer.objects.active = root_obj

export_path = r"E:\Unity\GUT FEELING\Assets\_Project\Models\wardrobe-wooden\source\Wardrobe_Operable.fbx"
bpy.ops.export_scene.fbx(
    filepath=export_path,
    use_selection=True,
    global_scale=1.0,
    apply_unit_scale=True,
    apply_scale_options='FBX_SCALE_NONE',
    axis_forward='-Z',
    axis_up='Y',
    bake_space_transform=True,
    mesh_smooth_type='FACE'
)
print("=== Successfully rebuilt Wardrobe_Operable.fbx with correct normals ===")
