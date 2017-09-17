package com.eightbitforest.delta.objects.walls;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.ShapeBuilder;

public class WallTriangleHalfTopLeft extends Wall {
    public WallTriangleHalfTopLeft(Level level, float x, float y) {
        super(level, x, y);
    }

    @Override
    protected void setShape(ShapeBuilder shape) {
        shape.setAsHalfTriangle();
    }
}
