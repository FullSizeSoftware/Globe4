shader_type spatial;
render_mode unshaded,wireframe;

uniform float outline_width : hint_range(0.0, 10.0, 0.1) = 0.0;
uniform vec3 wirecolor = vec3(1.0, 1.0, 1.0);

void vertex() {
    vec4 clip_position = PROJECTION_MATRIX * (MODELVIEW_MATRIX * vec4(VERTEX, 1.0));
    vec3 clip_normal = mat3(PROJECTION_MATRIX) * (mat3(MODELVIEW_MATRIX) * NORMAL);

    vec2 offset = normalize(clip_normal.xy) / VIEWPORT_SIZE * clip_position.w * outline_width * 2.0;

    clip_position.xy += offset;

    POSITION = clip_position;
}

void fragment() {
    ALBEDO = wirecolor;
}
