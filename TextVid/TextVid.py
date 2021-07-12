import json
from moviepy.editor import *
import sys

text_overlay_json_path = "./TextOverlays.json"
original_vid_path = "./NewVid.mp4"
edited_vid_path = "./NewVid_Edited.mp4"
text_overlays = []

if len(sys.argv) >= 2:
    text_overlay_json_path = sys.argv[1]

    if len(sys.argv) >= 3:
        original_vid_path = sys.argv[2]

    if len(sys.argv) >= 4:
        edited_vid_path = sys.argv[3]

print("Preparing...")

with open(text_overlay_json_path) as json_file:
    text_overlays = json.load(json_file)

video = VideoFileClip(original_vid_path)
audio = AudioFileClip(original_vid_path) 
video_clips = [video]

for overlay in text_overlays:
    video_clips.append(
        TextClip(overlay["Text"],fontsize=70,color='white')
            .set_position(overlay["Position"])
            .set_start(overlay["StartTime"])
            .set_end(overlay["StopTime"]))

result = CompositeVideoClip(video_clips) # Overlay text on video
result.set_audio(video.audio)

print("\nExporting...\n")

result.write_videofile(edited_vid_path, codec='libx264', preset="slow",)
result.close()
