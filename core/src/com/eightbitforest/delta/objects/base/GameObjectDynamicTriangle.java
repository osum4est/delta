package com.eightbitforest.delta.objects.base;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.SpriteBatch;
import com.badlogic.gdx.graphics.glutils.ShapeRenderer;
import com.badlogic.gdx.math.MathUtils;
import com.badlogic.gdx.math.Vector2;
import com.badlogic.gdx.physics.box2d.Body;
import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.physics.box2d.FixtureDef;
import com.badlogic.gdx.physics.box2d.PolygonShape;
import com.eightbitforest.delta.utils.BodyData;
import com.eightbitforest.delta.utils.G;
import com.eightbitforest.delta.utils.Globals;

/**
 * Created by osumf on 8/18/2015.
 */
public abstract class GameObjectDynamicTriangle extends GameObjectDynamic {

//    private float _linearDamping, _angularDamping, _density, _friction, _restitution, _triangleSize;
//    private short _category, _mask;

    public GameObjectDynamicTriangle(int id)
    {
        this(id, new BodyBuilder());
    }

    public GameObjectDynamicTriangle(int id, BodyBuilder body) {
        super(id, body);
    }

    public abstract Color getColor();

    @Override
    public void render(SpriteBatch batch) {
        batch.end();
        drawTriangle(G.i.shapeRenderer, G.i.TRIANGLE_HEIGHT, getColor());
        batch.begin();

    }

    public void drawTriangle(ShapeRenderer shapeRenderer, float height, Color color)
    {
        shapeRenderer.setProjectionMatrix(Globals.i.camera.combined);
        float side = (2 / (float) Math.sqrt(3)) * height;

        shapeRenderer.begin(ShapeRenderer.ShapeType.Filled);
        shapeRenderer.setColor(color);
        shapeRenderer.identity();
        shapeRenderer.translate(body.getPosition().x, body.getPosition().y, 0);
        shapeRenderer.rotate(0, 0, 1, body.getAngle() * MathUtils.radiansToDegrees);
        shapeRenderer.triangle(-side / 2, -height / 3, side / 2, -height / 3, 0, height / 3 * 2);
        shapeRenderer.end();
    }
}
