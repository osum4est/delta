package com.eightbitforest.delta.objects.walls;

import com.eightbitforest.delta.level.Level;
import com.eightbitforest.delta.objects.base.ShapeBuilder;
import com.eightbitforest.delta.utils.Constants;

public class WallBox extends Wall {
    public WallBox(Level level, float x, float y) {
        super(level, x, y);
    }

    @Override
    protected void setShape(ShapeBuilder shape) {
        shape.setAsRect(Constants.TRIANGLE_SIDE, Constants.TRIANGLE_HEIGHT);
    }
}
