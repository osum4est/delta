package com.eightbitforest.delta.objects;

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

    private float _linearDamping, _angularDamping, _density, _friction, _restitution;

    public GameObjectDynamicTriangle()
    {
        this(2.0f, 2.0f, 1.0f, 1.0f, 0.0f);
    }

    public GameObjectDynamicTriangle(float linearDamping, float angularDamping, float density, float friction, float restitution) {
        super(null, false);
        _linearDamping = linearDamping;
        _angularDamping = angularDamping;
        _density = density;
        _friction = friction;
        _restitution = restitution;

        body = getBody(new BodyDef(), new FixtureDef());
    }

    public abstract Color getColor();

    @Override
    Body getBody(BodyDef bdef, FixtureDef fdef) {
        Body body;

        bdef.type = BodyDef.BodyType.DynamicBody;
        bdef.position.set(0, 0);

        PolygonShape shape = getTriangleShape(G.i.TRIANGLE_HEIGHT);

        fdef.density = _density;
        fdef.friction = _friction;
        fdef.restitution = _restitution;
        fdef.shape = shape;

        body = Globals.i.world.createBody(bdef);
        body.createFixture(fdef);
        shape.dispose();

        body.setLinearDamping(_linearDamping);
        body.setAngularDamping(_angularDamping);

        body.setUserData(new BodyData(this));

        return body;
    }

    @Override
    public void render(SpriteBatch batch) {
        batch.end();
        drawTriangle(Globals.i.shapeRenderer, G.i.TRIANGLE_HEIGHT, getColor());
        batch.begin();

    }

    public PolygonShape getTriangleShape(float height)
    {
        float side = (2 / (float)Math.sqrt(3)) * height;

        PolygonShape shape = new PolygonShape();
        shape.set(new Vector2[]{
                new Vector2(-side / 2, -height / 3),
                new Vector2(side / 2, -height / 3),
                new Vector2(0, height / 3 * 2)});

        return shape;
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
