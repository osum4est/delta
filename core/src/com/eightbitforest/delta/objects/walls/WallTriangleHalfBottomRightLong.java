package com.eightbitforest.delta.objects.walls;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.ShapeBuilder;

public class WallTriangleHalfBottomRightLong extends Wall {
    public WallTriangleHalfBottomRightLong(Level level, float x, float y) {
        super(level, x, y);
    }

    @Override
    protected void setShape(ShapeBuilder shape) {
        shape.setAsHalfTriangleLong();
        shape.flip(true, true);
    }
}
