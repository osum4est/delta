package com.eightbitforest.delta.objects;

import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Ids;

public class Fuel extends GameObject {
    private Texture texture;
    private float textureResolution = 1000;
    private float radius;

    public Fuel(Level level, float x, float y) {
        super(level, Ids.FUEL, x, y, Colors.FUEL, false);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.StaticBody).setSensor(true)
                .setShape(new ShapeBuilder().setAsCircle(.4f)));
    }

    @Override
    protected void setupShape(ShapeBuilder shape) {
        radius = shape.getRadius();

        Pixmap pixmap = new Pixmap((int) (radius * textureResolution * 2), (int) (radius * textureResolution * 2), Pixmap.Format.RGBA8888);
        pixmap.setBlending(Pixmap.Blending.None);

        pixmap.setColor(getColor());
        pixmap.fillCircle((int) (radius * textureResolution), (int) (radius * textureResolution), (int) (radius * textureResolution));

        pixmap.setColor(0, 0, 0, 0);
        pixmap.fillCircle((int) (radius * textureResolution), (int) (radius * textureResolution), (int) (radius / 10 * 9 * textureResolution));

        pixmap.setColor(getColor());
        pixmap.fillCircle((int) (radius * textureResolution), (int) (radius * textureResolution), (int) (radius / 2 * textureResolution));

        texture = new Texture(pixmap);
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        batch.draw(texture, getX() - radius, getY() - radius, radius * 2, radius * 2);
    }
}
