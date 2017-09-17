package com.eightbitforest.delta.debug;

import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.GL30;
import com.badlogic.gdx.graphics.glutils.ImmediateModeRenderer20;
import com.badlogic.gdx.math.Matrix4;
import com.eightbitforest.delta.utils.Constants;

public class DebugGrid {
    private ImmediateModeRenderer20 lineRenderer;

    public DebugGrid() {
        lineRenderer = new ImmediateModeRenderer20(false, true, 0);
    }

    public void render(Matrix4 projection, int columns, int rows) {
        lineRenderer.begin(projection, GL30.GL_LINES);
        for (int col = -columns; col < columns; col++) {
            line(Constants.TRIANGLE_SIDE / 2 + col * Constants.TRIANGLE_SIDE,
                    Constants.TRIANGLE_HEIGHT / 3 * 2 + -rows * Constants.TRIANGLE_HEIGHT,
                    Constants.TRIANGLE_SIDE / 2 + col * Constants.TRIANGLE_SIDE,
                    Constants.TRIANGLE_HEIGHT / 3 * 2 + rows * Constants.TRIANGLE_HEIGHT - Constants.TRIANGLE_HEIGHT,
                    Color.BLACK);
        }
        for (int row = -rows; row < rows; row++) {
            line(Constants.TRIANGLE_SIDE / 2 + -columns * Constants.TRIANGLE_SIDE,
                    Constants.TRIANGLE_HEIGHT / 3 * 2 + row * Constants.TRIANGLE_HEIGHT,
                    Constants.TRIANGLE_SIDE / 2 + columns * Constants.TRIANGLE_SIDE - Constants.TRIANGLE_SIDE,
                    Constants.TRIANGLE_HEIGHT / 3 * 2 + row * Constants.TRIANGLE_HEIGHT,
                    Color.BLACK);
        }
        lineRenderer.end();
    }

    private void line(float x1, float y1, float x2, float y2, Color color) {
        lineRenderer.color(color);
        lineRenderer.vertex(x1, y1, 0);
        lineRenderer.color(color);
        lineRenderer.vertex(x2, y2, 0);
    }
}
