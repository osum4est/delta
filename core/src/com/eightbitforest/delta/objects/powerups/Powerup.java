package com.eightbitforest.delta.objects.powerups;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.Pixmap;
import com.badlogic.gdx.graphics.Texture;
import com.badlogic.gdx.graphics.g2d.Batch;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObject;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.objects.generic.Player;
import com.eightbitforest.delta.utils.Constants;

public abstract class Powerup extends GameObject {
    private Texture texture;
    private float radius;

    public Powerup(Level level, int id, float x, float y, Color color) {
        super(level, id, x, y, color, false);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.StaticBody).setSensor(true)
                .setShape(new ShapeBuilder().setAsCircle(.4f)));
    }

    @Override
    protected void setupShape(ShapeBuilder shape) {
        radius = shape.getRadius();

        Pixmap pixmap = new Pixmap((int) (radius * Constants.TEXTURE_RESOLUTION * 2), (int) (radius * Constants.TEXTURE_RESOLUTION * 2), Pixmap.Format.RGBA8888);
        pixmap.setBlending(Pixmap.Blending.None);

        pixmap.setColor(getColor());
        pixmap.fillCircle((int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius * Constants.TEXTURE_RESOLUTION));

        pixmap.setColor(0, 0, 0, 0);
        pixmap.fillCircle((int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius / 10 * 9 * Constants.TEXTURE_RESOLUTION));

        pixmap.setColor(getColor());
        pixmap.fillCircle((int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius * Constants.TEXTURE_RESOLUTION), (int) (radius / 2 * Constants.TEXTURE_RESOLUTION));

        texture = new Texture(pixmap);
    }

    @Override
    public void draw(Batch batch, float parentAlpha) {
        batch.draw(texture, getX() - radius, getY() - radius, radius * 2, radius * 2);
    }

    public abstract void activate(Player player);
}
