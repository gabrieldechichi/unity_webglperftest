## Summary

This is a minimal Unity project I set up to compare Unity's performance against my [C engine](https://x.com/gdechichi/status/1934323963167178965) for running *on the web*.

![C vs Unity comparison](./img.png)

## Unity build settings + small optimizations

The goal of this project was to evaluate Unity's "out of the box" Animator implementation, i.e simulate what most developers would experience.

Still, I made sure to choose reasonable settings to make Unity perform "as best as possible" given the target architecture (WebGL).

Web Build Settings:

- Release Build
- Code Optimization: Runtime speed
- Exceptions disabled
- Stacktraces disabled
- Strip Engine Code enabled
- Managed Stripping Level: Minimal (tried High and build failed, might be worth trying other options)

Graphics / Animation "optimizations":

- Enabled Strip Bones and Optimize Game Objects on `running.fbx`
- Use custom Phong shader (direction + ambient light only) similar to the one from my custom engine
- Mobile URP settings
- Disabled Post processing on Main Camera
