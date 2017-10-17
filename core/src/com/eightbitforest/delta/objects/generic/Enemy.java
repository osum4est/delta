package com.eightbitforest.delta.objects.generic;

import com.badlogic.gdx.math.Vector2;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.GameObjectPolygon;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Colors;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Ids;

public class Enemy extends GameObjectPolygon {
    public Enemy(Level level, float x, float y) {
        super(level, Ids.ENEMY, x, y, Colors.GREEN, false);
        setBody(new BodyBuilder()
                .setShape(new ShapeBuilder().setAsTriangle(Constants.TRIANGLE_HEIGHT * 1f))
                .setLinearDamping(.4f));
    }

    /**
     * Very basic AI, simply moving towards player
     */
    @Override
    public void act(float delta) {
        super.act(delta);

        Vector2 direction = new Vector2(
                getLevel().getPlayer().getX() - getX(),
                getLevel().getPlayer().getY() - getY()
        );
        Vector2 force = new Vector2(0, 1);
        force.rotateRad(body.getAngle()).nor();
        body.applyForceToCenter(force.x * .4f, force.y * .4f, true);

        setRotation(direction.angle() - 90);
    }
}
