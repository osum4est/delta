package com.eightbitforest.delta.utils;

import com.badlogic.gdx.math.MathUtils;
import com.eightbitforest.delta.objects.base.GameObject;

public class Animator {
    private GameObject gameObject;

    private boolean enabled;

    private float scaleSpeed;
    private float fadeSpeed;
    private float moveSpeed;
    private float moveX;
    private float moveY;

    public Animator(GameObject gameObject) {
        this.gameObject = gameObject;
        this.enabled = true;
    }

    public Animator scale(float scaleSpeed) {
        this.scaleSpeed = scaleSpeed;
        return this;
    }

    public Animator fade(float fadeSpeed) {
        this.fadeSpeed = fadeSpeed;
        return this;
    }

    public Animator lerp(float speed, float x, float y) {
        this.moveSpeed = speed;
        this.moveX = x;
        this.moveY = y;
        return this;
    }

    public void update(float delta) {
        if (!enabled)
            return;

        // Scale
        if (scaleSpeed != 0) {
            float newScale = gameObject.getScaleX() + delta * scaleSpeed;
            if (newScale < 0)
                newScale = 0;
            gameObject.setScale(newScale);
        }

        // Fade
        if (fadeSpeed != 0) {
            float newA = gameObject.getColor().a - delta * fadeSpeed;
            if (newA < 0)
                newA = 0;
            gameObject.getColor().a = newA;
        }

        // Translate
        if (moveSpeed != 0) {
            gameObject.setPosition(
                    MathUtils.lerp(gameObject.getX(), moveX, moveSpeed),
                    MathUtils.lerp(gameObject.getY(), moveY, moveSpeed)
            );
        }
    }

    public void enable() {
        enabled = true;
    }

    public void disable() {
        enabled = false;
    }
}
