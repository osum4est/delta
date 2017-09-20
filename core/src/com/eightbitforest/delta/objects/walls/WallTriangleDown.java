package com.eightbitforest.delta.objects.walls;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.ShapeBuilder;

public class WallTriangleDown extends Wall {
    public WallTriangleDown(Level level, float x, float y) {
        super(level, x, y);
    }

    @Override
    protected void setShape(ShapeBuilder shape) {
        shape.flip(false, true);
    }
}