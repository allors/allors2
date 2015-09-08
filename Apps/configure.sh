#!/bin/sh

xbuild Tasks/Merge.proj /verbosity:minimal

xbuild Base.sln /target:Clean /verbosity:minimal
xbuild Tasks/GenerateMeta.proj /verbosity:minimal

xbuild Base.sln /target:Meta:Rebuild /p:Configuration="Debug" /verbosity:minimal
xbuild Tasks/GenerateDomain.proj /verbosity:minimal
