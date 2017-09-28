package com.eightbitforest.delta.objects.walls;

import com.badlogic.gdx.physics.box2d.BodyDef;
import com.badlogic.gdx.utils.JsonValue;
import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.BodyBuilder;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Constants;
import com.eightbitforest.delta.utils.Utils;

public class WallBox extends Wall {
    private float speed;
    private float angularSpeed;

    private float dX;
    private float dY;

    private float startX;
    private float startY;

    private float dirX = 1;
    private float dirY = 1;

    public WallBox(Level level, float x, float y) {
        super(level, x, y);
    }

    @Override
    protected void setShape(ShapeBuilder shape) {
        shape.setAsRect(Constants.TRIANGLE_SIDE, Constants.TRIANGLE_HEIGHT);
        setY(getY() + Constants.TRIANGLE_HEIGHT / 3 / 2);
    }

    @Override
    public void setProperties(JsonValue json) {
        float width = Utils.getJsonFloatOrDefault(json, "width", 1);
        float height = Utils.getJsonFloatOrDefault(json, "height", 1) * Constants.TRIANGLE_HEIGHT;

        speed = Utils.getJsonFloatOrDefault(json, "speed", 1);
        angularSpeed = Utils.getJsonFloatOrDefault(json, "angularSpeed", 0);

        dX = Utils.getJsonFloatOrDefault(json, "dX", 0);
        dY = Utils.getJsonFloatOrDefault(json, "dY", 0);

        setRotation(Utils.getJsonFloatOrDefault(json, "angle", 0));

        getLevel().getWorld().destroyBody(body);
        setBody(new BodyBuilder().setBodyType(BodyDef.BodyType.KinematicBody).setShape(
                new ShapeBuilder().setAsRect(width, height)
        ));

        startX = getX();
        startY = getY();
    }

    @Override
    public void act(float delta) {
        super.act(delta);

        if (getX() > startX + dX)
            dirX = -1;
        else if (getX() < startX)
            dirX = 1;
        setX(getX() + speed * delta * dirX);

        if (getY() > startY + dY)
            dirY = -1;
        else if (getY() < startY)
            dirY = 1;
        setY(getY() + speed * delta * dirY);

        setRotation(getRotation() + angularSpeed * delta * 360);
    }
}